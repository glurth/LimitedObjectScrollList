namespace EyE.Unity.UI.Templates
{
    /// <inheritdoc/>
    /// <summary>
    /// Concrete implementation of a LimitedObjectScrollListBase.  It displays a list that consists of strings, via a TextDisplay prefabs.
    /// While this list may contain thousands of items, only a few TextDisplay objects, just enough to fill the viewport, will be created in the scene
    /// </summary>
    public class LimitedTextObjectScrollListTemplate : LimitedObjectScrollList<string, TextDisplay>
    { }
}