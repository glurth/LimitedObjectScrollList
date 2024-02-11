using UnityEngine;
using TMPro;

namespace EyE.Unity.UI.Templates
{
    /// <summary>
    /// Provides access to a TextMeshProUGUI object via the IDisplay<string> interface.  To use it, just add this component to an object with a TextMeshProUGUI component.
    /// </summary>
    public class TextDisplay : MonoBehaviour, IDisplay<string>
    {
        private void Reset()
        {
            if(textControl==null)
                textControl = GetComponent<TextMeshProUGUI>();
        }

        public TextMeshProUGUI textControl;
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