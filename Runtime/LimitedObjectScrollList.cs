using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace EyE.Unity.UI
{
    /// <summary>
    /// Represents a scrollable list of unlimited objects, and displays them using a limited/minimal number of instantiated UI Objects.
    /// To use this class, call the SetList function, and pass in the list you want displayed.  This class will then automatically instantiate and/or place a minimal number of prefabs to display the list elements.
    /// You may, if the prefab supports it, subscribe to UI events that happen on/to any list elements via the ``UnityEvent<int> onXXXEvent`` members. When triggered by the user, these events will pass an index to the list element the event occurred on, to subscribers.
    /// </summary>
    /// <typeparam name="TListElementType">The type of elements in the full list.</typeparam>
    /// <typeparam name="TLineElementPreFabType">The type of prefab used to display list elements. 
    /// It must implement the <see cref="IDisplay{TListElementType}"/> interface and must be a Monobehavior, to ensure proper functionality.  
    /// If you would like to detect UI Events on the list elements, the Prefab must provide the appropriate interface(s): ITriggerOnSelect, ITriggerOnHover, ITriggerOnClick. </typeparam>
    public class LimitedObjectScrollList<TListElementType,TLineElementPreFabType>:MonoBehaviour where TLineElementPreFabType : MonoBehaviour,IDisplay<TListElementType>
    {
        /// <summary>
        /// The prefab used to display individual list elements.
        /// </summary>
        public TLineElementPreFabType lineElementPreFab;
        /// <summary>
        /// The height of each line element in the list.
        /// </summary>
        public float lineElementHeight=40;
        /// <summary>
        /// The ScrollRect responsible for scrolling the list.
        /// </summary>
        public ScrollRect controller;
        /// <summary>
        /// The RectTransform representing the view area of the scrollable list- this is usually a child AND member of the ScrollRect
        /// </summary>
        public RectTransform viewRect;
        /// <summary>
        /// The RectTransform representing the content area of the scrollable list- this is usually a child AND member of the ScrollRect
        /// </summary>
        public RectTransform contentTransform;

        //stores a reference to the full list of data to be displayed
        List<TListElementType> fullList;
        //stores a list of TLineElementPreFabType that have been instantiated already.  This list should not change in size unless the visible height of the scroll view changes- and even then element will only be added.  Undisplayed/unneeded  elements are deactivated.
        List<TLineElementPreFabType> instantiatedDisplayElements = new List<TLineElementPreFabType>();

        /// <summary>
        /// Gets the display element at the specified index in the full list.
        /// Returns null if the element is not currently displayed.
        /// </summary>
        /// <param name="fullListIndex">The index of the element in the full list.</param>
        /// <returns>The displayed element, or null if not currently displayed.</returns>
        public TLineElementPreFabType GetDisplayElement(int fullListIndex)
        {
            int displayIndex = fullListIndex - currentStartIndex;
            if (displayIndex < 0 || displayIndex >= instantiatedDisplayElements.Count)
                return null;
            return instantiatedDisplayElements[displayIndex];
        }

        /// <summary>
        /// Event triggered when the pointer enters an element in the list.
        /// The int parameter represents the index of the element in the full list.
        /// Note: The prefab used (TLineElementPreFabType) must implement the ITriggerOnHover interface for this event to work.
        /// </summary>
        public UnityEvent<int> onSelectEvent { get; } = new UnityEvent<int>();
        void InternalHandleElementSelect(int displayElementNumber)
        {
            onSelectEvent.Invoke(displayElementNumber + currentStartIndex);
        }
        /// <summary>
        /// Event triggered when the pointer exits an element in the list.
        /// The int parameter represents the index of the element in the full list.
        /// Note: The prefab used (TLineElementPreFabType) must implement the ITriggerOnHover interface for this event to work.
        /// </summary>
        public UnityEvent<int> onPointerEnterEvent { get; } = new UnityEvent<int>();
        void InternalHandleElementPointerEnter(int displayElementNumber)
        {
            onPointerEnterEvent.Invoke(displayElementNumber + currentStartIndex);
        }

        /// <summary>
        /// Event triggered when the pointer exits an element in the list.
        /// The int parameter represents the index of the element in the full list.
        /// Note: The prefab used (TLineElementPreFabType) must implement the ITriggerOnHover interface for this event to work.
        /// </summary>
        public UnityEvent<int> onPointerExitEvent { get; } = new UnityEvent<int>();
        void InternalHandleElementPointerExit(int displayElementNumber)
        {
            onPointerExitEvent.Invoke(displayElementNumber + currentStartIndex);
        }

        /// <summary>
        /// Event triggered when an element in the list is clicked.
        /// The int parameter represents the index of the clicked element in the full list.
        /// Note: The prefab used (TLineElementPreFabType) must implement the ITriggerOnClick interface for this event to work.
        /// </summary>
        public UnityEvent<int> onClickEvent { get; } = new UnityEvent<int>();
        void InternalHandleElementClick(int displayElementNumber)
        {
            Debug.Log("Limited list onclick display element: " + displayElementNumber);
            onClickEvent.Invoke(displayElementNumber + currentStartIndex);
        }

        public UnityEvent<int> onElementInView { get; } = new UnityEvent<int>();


        /// <summary>
        /// Gets a value indicating whether any item in the scroll list currently has focus.
        /// </summary>
        /// <returns>True if any item in the scroll list has focus; otherwise, false.</returns>
        public bool HasFocus()
        {
            foreach (TLineElementPreFabType viewElement in instantiatedDisplayElements)
                if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == viewElement.gameObject)
                    return true;
            return false;
        }

        /// <summary>
        /// Sets the full list of elements to be displayed in the scroll list.
        /// </summary>
        /// <param name="fullList">The full list of elements.</param>
        public void SetList(List<TListElementType> fullList)
        {
            Canvas.ForceUpdateCanvases();
            this.fullList = fullList;
            int count = fullList.Count;
            float totalListHeight = count * lineElementHeight;
           // totalListHeight += (count-1) * verticalSpacing;
            contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, totalListHeight);

            float viewHeight = viewRect.rect.height;//.sizeDelta.y;
            float numElementsThatFitInView = viewHeight / lineElementHeight;
            int numberOfElementsToCreate = Mathf.CeilToInt(numElementsThatFitInView)+2;
            if (count < numberOfElementsToCreate)
                numberOfElementsToCreate = count;

            for (int i = 0; i < numberOfElementsToCreate; i++)
            {
                if (i >= instantiatedDisplayElements.Count)//if element does not exist yet
                {
                    TLineElementPreFabType newLineElement = Instantiate<TLineElementPreFabType>(lineElementPreFab, contentTransform);
                    instantiatedDisplayElements.Add(newLineElement);
                    int displayElementIndex = i;  // this is not the index to the full list element, rather it is the index of the display element of which we have a limited number
                                                  //we put i it into  new var to "descope" it for use below.

                    ITriggerOnClick clicker = newLineElement.GetComponentInChildren<ITriggerOnClick>();
                    if (clicker != null)
                        clicker.onClickEvent.AddListener(() => { InternalHandleElementClick(displayElementIndex); });
                    ITriggerOnSelect selectable = newLineElement.GetComponentInChildren<ITriggerOnSelect>();
                    if (selectable != null)
                        selectable.onSelectEvent.AddListener(() => { InternalHandleElementSelect(displayElementIndex); });
                    ITriggerOnHover hoverable = newLineElement.GetComponentInChildren<ITriggerOnHover>();
                    if (hoverable != null)
                    {
                        hoverable.onPointerEnterEvent.AddListener(() => { InternalHandleElementPointerEnter(displayElementIndex); });
                        hoverable.onPointerExitEvent.AddListener(() => { InternalHandleElementPointerExit(displayElementIndex); });
                    }
                }
                TLineElementPreFabType displayElement = instantiatedDisplayElements[i];
                displayElement.gameObject.SetActive(true);
                displayElement.Display(fullList[i]);
               
                onElementInView.Invoke(i);
            }
            //if any other instantiatedDisplayElements, that were created previously, exist: deactivate them.
            for (int i = numberOfElementsToCreate; i < instantiatedDisplayElements.Count; i++)
            {
                instantiatedDisplayElements[i].gameObject.SetActive(false);
            }
        }

        float lastScrollPos = float.NegativeInfinity; //used to determine if elements need to reassigned new fullList values
        int currentStartIndex = 0;// fullList index of the current topmost/first displayed element
        
        /// <summary>
        /// Updates the scroll list, refreshing the displayed elements based on changes in the current scroll position.
        /// </summary>
        public void Update()
        {
            if (fullList == null || fullList.Count==0) return;
            float scrollPos = contentTransform.localPosition.y;
            //see if the user has scrolled enough that we need to reassign/update display values.  if not, do nothing/return
            if (lastScrollPos!= float.NegativeInfinity && Mathf.Abs(scrollPos - lastScrollPos) < lineElementHeight) return;
            lastScrollPos = scrollPos;
            int startIndex = Mathf.FloorToInt(scrollPos / lineElementHeight);
            if (startIndex < 0) startIndex = 0;
            currentStartIndex = startIndex;// record in a member for use by event triggers
            float startIndexPos = startIndex * lineElementHeight;
            // float endIndexPos = startIndexPos + (instantiatedDisplayElements.Count * lineElementHeight);
            
            // move all display elements and populate
            float posCounter = startIndexPos + lineElementHeight/2f;
            for (int i = 0; i < instantiatedDisplayElements.Count; i++)
            {
                int fullListIndex = startIndex + i;
                if (fullListIndex < fullList.Count)
                {
                    TLineElementPreFabType displayElement = instantiatedDisplayElements[i];
                    Vector3 currentPos = displayElement.transform.localPosition;
                    currentPos.y = -posCounter;
                    displayElement.transform.localPosition = currentPos;
                    displayElement.Display(fullList[fullListIndex]);
                    posCounter += lineElementHeight;
                    onElementInView.Invoke(fullListIndex);
                }
            }


        }

    }
}