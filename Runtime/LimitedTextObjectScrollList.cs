using System.Collections.Generic;

namespace EyE.Unity.UI
{
    /// <inheritdoc/>
    /// <summary>
    /// Concrete implementation of a LimitedObjectScrollListBase.  It displays a list that consists of strings, via a TextDisplay prefabs.
    /// </summary>
    public class LimitedTextObjectScrollList : LimitedObjectScrollListBase<string, TextDisplay>
    {
        public List<string> stringList;
        
        //for testing
        private void Start()
        {
            for (int i = 0; i < 10000; i++)
                stringList.Add(i.ToString());
            SetList(stringList);
        }
    }
}