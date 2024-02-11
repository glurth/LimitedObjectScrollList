using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace EyE.Unity.UI.Templates
{
    /// <summary>
    /// Provides access to a TMP_InputField object via the IDisplay<string>, ITriggerOnValueChange<string> and ITriggerOnValueEditEnd<string> interfaces.  To use it, just add this component to an object with a TMP_InputField component.
    /// </summary>
    public class TextIO : MonoBehaviour, IDisplay<string>, ITriggerOnValueChange<string>, ITriggerOnValueEditEnd<string>//, ITriggerOnValueValidate<string>
    {
        public TMP_InputField textControl;
        private void Reset()
        {
            if (textControl == null)
                textControl = GetComponent<TMP_InputField>();
        }


        public UnityEvent<string> onValueChanged { get { return textControl.onValueChanged; } }
        public UnityEvent<string> onValueEditEnd { get { return textControl.onEndEdit; } }
        //public UnityEvent<string> onValueValidate { get { return textControl.onValidateInput; } }

        /// <summary>
        /// set the TextMeshProUGUI member to display the provided string
        /// </summary>
        /// <param name="obj">string that will be displayed</param>
        public void Display(string obj)
        {
            textControl.text = obj;
        }
    }
}