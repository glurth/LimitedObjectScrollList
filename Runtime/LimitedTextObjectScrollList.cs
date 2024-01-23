using System.Collections.Generic;

namespace EyE.Unity.UI
{
    /// <inheritdoc/>
    /// <summary>
    /// Concrete implementation of a LimitedObjectScrollListBase.  It displays a list that consists of strings, via a TextDisplay prefabs.
    /// </summary>
    public class LimitedTextObjectScrollList : LimitedObjectScrollList<string, TextDisplay>
    {

        //for testing.  while this list may contain 10k items, only a few TextDisplay objects, just enough to fill the viewport, will be created in the scene,
        public List<string> stringList;
        private void Start()
        {
            for (int i = 0; i < 10000; i++)
                stringList.Add(i.ToString());
            SetList(stringList);
        }
    }
}