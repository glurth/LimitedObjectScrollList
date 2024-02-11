using System.Collections.Generic;
using EyE.Unity.UI.Templates;
namespace EyE.Unity.UI.Examples
{

    /// <inheritdoc/>
    /// <summary>
    /// Example showing a list of some values being displayed.
    /// </summary>
    public class LimitedTextObjectScrollListTester : LimitedTextObjectScrollListTemplate
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