using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace EyE.Unity.UI
{
    /// <summary>
    /// Represents a scrollable list of unlimited objects, and displays them using a limited/minimal number of instantiated UI Objects.
    /// It is recommended that you create your own, concrete class, derived from this one, and add it as a component to an object with a scroll list.
    /// To use this class, call the SetList function, and pass in the list you want displayed.  This class will then automatically instantiate and/or place a minimal number of prefabs to display the list elements.
    /// You may, if the prefab supports it, subscribe to UI events that happen on/to any list elements via the ``UnityEvent<int> onXXXEvent`` members. 
    /// When triggered by the user, these events will pass an index to the list element the event occurred on, to subscribers.
    /// </summary>
    /// <typeparam name="TListElementType">The type of elements in the full list.</typeparam>
    /// <typeparam name="TLineElementPreFabType">The type of prefab used to display list elements. 
    /// It must implement the <see cref="IDisplay{TListElementType}"/> interface and must be a Monobehavior, to ensure proper functionality.  
    /// If you would like to detect UI Events on the list elements, the Prefab must provide the appropriate interface(s): ITriggerOnSelect, ITriggerOnHover, ITriggerOnClick, ITriggerOnValueChange<TListElementType>,ITriggerOnValueEditEnd<TListElementType>, ITriggerOnValueValidate<TListElementType>. </typeparam>
    public class LimitedObjectScrollList<TListElementType,TLineElementPreFabType>:MonoBehaviour where TLineElementPreFabType : MonoBehaviour,IDisplay<TListElementType>
    {
        /// <summary>
        /// The prefab used to display individual list elements.
        /// </summary>
        [Tooltip("The prefab used to display individual list elements.")]
        public TLineElementPreFabType lineElementPreFab;
        /// <summary>
        /// The height of each line element in the list.
        /// </summary>
        [Tooltip("The height of each line element in the list.")]
        public float lineElementHeight=40;
        /// <summary>
        /// The ScrollRect responsible for scrolling the list.
        /// </summary>
        [Tooltip("The ScrollRect responsible for scrolling the list.")]
        public ScrollRect controllerScrollRect;

        /// <summary>
        /// The RectTransform representing the view area of the scrollable list- this is usually a child AND member of the ScrollRect
        /// </summary>
        RectTransform viewRectTransform=>controllerScrollRect.viewport;
        /// <summary>
        /// The RectTransform representing the content area of the scrollable list- this is usually a child AND member of the ScrollRect
        /// </summary>
        RectTransform contentRectTransform=>controllerScrollRect.content;

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
        /// Event triggered when an element in the list becomes 'selected'.
        /// The int parameter represents the index of the element in the full list.
        /// Note: The prefab used (TLineElementPreFabType) must implement the <see cref="ITriggerOnSelect"/> interface for this event to work.
        /// </summary>
        public UnityEvent<int> onSelectEvent => _onSelectEvent;
        [SerializeField][Tooltip("Event triggered when an element in the list becomes 'selected'.")]
        private UnityEvent<int> _onSelectEvent = new UnityEvent<int>();
        //this function is added as a listener to prefabs when they are instantiated.
        void InternalHandleElementSelect(int displayElementNumber)
        {
            onSelectEvent.Invoke(displayElementNumber + currentStartIndex);
        }

        /// <summary>
        /// Event triggered when the pointer enters an element in the list.
        /// The int parameter represents the index of the element in the full list.
        /// Note: The prefab used (TLineElementPreFabType) must implement the <see cref="ITriggerOnHover"/> interface for this event to work.
        /// </summary>
        public UnityEvent<int> onPointerEnterEvent => _onPointerEnterEvent;
        [SerializeField][Tooltip("Event triggered when the pointer enters an element in the list.")]
        private UnityEvent<int> _onPointerEnterEvent = new UnityEvent<int>();
        //this function is added as a listener to prefabs when they are instantiated.
        void InternalHandleElementPointerEnter(int displayElementNumber)
        {
            //add currentStartIndex to convert from display index to fullList index
            onPointerEnterEvent.Invoke(displayElementNumber + currentStartIndex);
        }

        /// <summary>
        /// Event triggered when the pointer exits an element in the list.
        /// The int parameter represents the index of the element in the full list.
        /// Note: The prefab used (TLineElementPreFabType) must implement the <see cref="ITriggerOnHover"/> interface for this event to work.
        /// </summary>
        public UnityEvent<int> onPointerExitEvent => _onPointerExitEvent;
        [SerializeField][Tooltip("Event triggered when the pointer exits an element in the list.")]
        private UnityEvent<int> _onPointerExitEvent = new UnityEvent<int>();
        //this function is added as a listener to prefabs when they are instantiated.
        void InternalHandleElementPointerExit(int displayElementNumber)
        {
            onPointerExitEvent.Invoke(displayElementNumber + currentStartIndex);
        }

        /// <summary>
        /// Event triggered when an element in the list is clicked.
        /// The int parameter represents the index of the clicked element in the full list.
        /// Note: The prefab used (TLineElementPreFabType), must implement the <see cref="ITriggerOnClick"/> interface for this event to work.
        /// </summary>
        public UnityEvent<int> onClickEvent => _onClickEvent;
        [SerializeField][Tooltip("Event triggered when an element in the list is clicked.")]
        private UnityEvent<int> _onClickEvent = new UnityEvent<int>();
        //this function is added as a listener to prefabs when they are instantiated.
        void InternalHandleElementClick(int displayElementNumber)
        {
            onClickEvent.Invoke(displayElementNumber + currentStartIndex);
        }

        /// <summary>
        /// Event triggered when an element in the list becomes visible on screen.  The int parameter represents the index into the full list.
        /// </summary>
        public UnityEvent<int> onElementInView { get; } = new UnityEvent<int>();

        /// <summary>
        /// When set to true onValueChangedEvent invocations will effect the contents of the fullList.
        /// </summary>
        [Tooltip("When set to true onValueChangedEvent invocations will effect the contents of the fullList.")]
        public bool isReadOnly = true;

        /// <summary>
        /// Event triggered when an element in the list has it's value changed from ANY source.
        /// The int parameter represents the index of the clicked element in the full list.
        /// The TListElementType parameter passes the new changed value.
        /// Note: The prefab used (TLineElementPreFabType), must implement the <see cref="ITriggerOnValueChange<TListElementType>"/> interface for this event to work.
        /// </summary>
        public UnityEvent<int, TListElementType> onValueChangedEvent => _onValueChangedEvent;
        [SerializeField]
        [Tooltip("Event triggered when the user changes the value of an element on the list.")]
        private UnityEvent<int, TListElementType> _onValueChangedEvent = new UnityEvent<int, TListElementType>();
        //this function is added as a listener to prefabs when they are instantiated.
        void InternalHandleElementValueChanged(int displayElementNumber, TListElementType newValue)
        {
          //  Debug.Log("Limited list display element value changed: " + displayElementNumber);
            int index = displayElementNumber + currentStartIndex;
            if (!isReadOnly)
            {
                onValueChangedEvent.Invoke(index, newValue);
            }
            else
            {
                instantiatedDisplayElements[displayElementNumber].Display(fullList[index]);
            }
        }

        /// <summary>
        /// Event triggered when the user has finished editing the value of an element on the list.
        /// The int parameter represents the index of the clicked element in the full list.
        /// The TListElementType parameter passes the new changed value.
        /// Note: The prefab used (TLineElementPreFabType), must implement the <see cref="ITriggerOnValueChange<TListElementType>"/> interface for this event to work.
        /// </summary>
        public UnityEvent<int, TListElementType> onValueEditEndEvent => _onValueEditEndEvent;
        [SerializeField]
        [Tooltip("Event triggered when the user has finished editing the value of an element on the list.")]
        private UnityEvent<int, TListElementType> _onValueEditEndEvent = new UnityEvent<int, TListElementType>();
        //this function is added as a listener to prefabs when they are instantiated.
        void InternalHandleElementEditEnd(int displayElementNumber, TListElementType newValue)
        {
            Debug.Log("Limited list display element value edit complete: " + displayElementNumber);
            int index = displayElementNumber + currentStartIndex;
            if (!isReadOnly)
            {
                fullList[index] = newValue;
                onValueEditEndEvent.Invoke(index, newValue);
            }
            else
            {
                instantiatedDisplayElements[displayElementNumber].Display(fullList[index]);
            }
        }

        /*/// <summary>
        /// Event triggered when an element in the list has it's value changed by the user.
        /// The int parameter represents the index of the clicked element in the full list.
        /// The TListElementType parameter passes the new changed value.
        /// Note: The prefab used (TLineElementPreFabType), must implement the <see cref="ITriggerOnValueChange<TListElementType>"/> interface for this event to work.
        /// </summary>
        public UnityEvent<int, TListElementType> onValidateEvent => _onValidateEvent;
        [SerializeField]
        [Tooltip("Event triggered when the user has finished editing the value of an element on the list.")]
        private UnityEvent<int, TListElementType> _onValidateEvent = new UnityEvent<int, TListElementType>();
        //this function is added as a listener to prefabs when they are instantiated.
        void InternalHandleElementValidate(int displayElementNumber, TListElementType newValue)
        {
            Debug.Log("Limited list display element value validate: " + displayElementNumber);
            int index = displayElementNumber + currentStartIndex;
            if (!isReadOnly)
            {
                _onValidateEvent.Invoke(index, newValue);
            }
            else
            {
                instantiatedDisplayElements[displayElementNumber].Display(fullList[index]);
            }
        }
        */

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
        /// Main Function: Sets the full list of elements to be displayed in the scroll list.
        /// </summary>
        /// <param name="fullList">The full list of elements.</param>
        public void SetList(List<TListElementType> fullList)
        {
            Canvas.ForceUpdateCanvases();
            this.fullList = fullList;
            int count = fullList.Count;
            float totalListHeight = count * lineElementHeight;
           // totalListHeight += (count-1) * verticalSpacing;
            contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, totalListHeight);

            float viewHeight = viewRectTransform.rect.height;//.sizeDelta.y;
            float numElementsThatFitInView = viewHeight / lineElementHeight;
            int numberOfElementsToCreate = Mathf.CeilToInt(numElementsThatFitInView)+2;
            if (count < numberOfElementsToCreate)
                numberOfElementsToCreate = count;

            for (int i = 0; i < numberOfElementsToCreate; i++)
            {
                if (i >= instantiatedDisplayElements.Count)//if element does not exist yet
                {
                    TLineElementPreFabType newLineElement = Instantiate<TLineElementPreFabType>(lineElementPreFab, contentRectTransform);
                    instantiatedDisplayElements.Add(newLineElement);
                    int displayElementIndex = i;  // this is not the index to the full list element, rather it is the index of the display element of which we have a limited number
                                                  //we put i it into  new var to "descope" it for use below.

                    //Add Listeners to any/all available Triggers on the newly instantiated list element prefab
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
                    ITriggerOnValueChange<TListElementType> changeable = newLineElement.GetComponentInChildren<ITriggerOnValueChange<TListElementType>>();
                    if (changeable != null)
                        changeable.onValueChanged.AddListener((TListElementType data) => { InternalHandleElementValueChanged(displayElementIndex, data); });

                    ITriggerOnValueEditEnd<TListElementType> editEndable = newLineElement.GetComponentInChildren<ITriggerOnValueEditEnd<TListElementType>>();
                    if (editEndable != null)
                        editEndable.onValueEditEnd.AddListener((TListElementType data) => { InternalHandleElementEditEnd(displayElementIndex, data); });

                    /*ITriggerOnValueValidate<TListElementType> validatable = newLineElement.GetComponentInChildren<ITriggerOnValueValidate<TListElementType>>();
                    if (validatable != null)
                        validatable.onValueValidate.AddListener((TListElementType data) => { InternalHandleElementValidate(displayElementIndex, data); });*/
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
            float scrollPos = contentRectTransform.localPosition.y;
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