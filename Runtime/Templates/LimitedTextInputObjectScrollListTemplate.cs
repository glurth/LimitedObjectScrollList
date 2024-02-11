namespace EyE.Unity.UI.Templates
{
    /// <inheritdoc/>
    /// <summary>
    /// Concrete implementation of a LimitedObjectScrollList.  It displays a list that consists of strings, via a TextIO prefabs.
    /// While this list may contain thousands of items, only a few TextIO objects, just enough to fill the viewport, will be created in the scene
    /// </summary>
    public class LimitedTextInputObjectScrollListTemplate : LimitedObjectScrollList<string, TextIO>
    { }
}