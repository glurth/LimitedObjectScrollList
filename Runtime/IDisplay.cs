namespace EyE.Unity.UI
{
    /// <summary>
    /// Base generic interface that provides a standard was to pass objects to classes that will display them on screen.
    /// </summary>
    /// <typeparam name="T">The type of object that will be passed/displayed</typeparam>
    public interface IDisplay<T>
    {
        void Display(T obj);
    }
}