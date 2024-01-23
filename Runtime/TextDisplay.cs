using UnityEngine;
using TMPro;
namespace EyE.Unity.UI
{
    /// <summary>
    /// Provides access to a TextMeshProUGUI object via the IDisplay<string> interface
    /// </summary>
    public class TextDisplay : MonoBehaviour, IDisplay<string>
    {
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