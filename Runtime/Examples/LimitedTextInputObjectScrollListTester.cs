using System.Collections.Generic;
using EyE.Unity.UI.Templates;

namespace EyE.Unity.UI.Examples
{

    /// <inheritdoc/>
    /// <summary>
    /// Concrete implementation of a LimitedObjectScrollList.  It displays a list that consists of strings, via a TextIO prefabs.
    /// While this list may contain thousands of items, only a few TextIO objects, just enough to fill the viewport, will be created in the scene
    /// </summary>
    public class LimitedTextInputObjectScrollListTester : LimitedTextInputObjectScrollListTemplate
    {
        /// <summary>
        /// This is the list that will be displayed by the LimitedObjectScrollList.
        /// </summary>
        public List<string> stringList;
        /// <summary>
        /// populate the list with an additional 10k values, for testing/example purposes.
        /// </summary>
        private void Start()
        {
            for (int i = 0; i < 10000; i++)
                stringList.Add(i.ToString());
            SetList(stringList); // call this base class function to assign the values to be displayed.
        }
    }
}