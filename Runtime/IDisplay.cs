namespace EyE.Unity.UI
{
    /// <summary>
    /// Base generic interface that provides a standard way to pass objects to classes that will display them on screen.
    /// Implementations of this interface are intended to be put on a MonoBehaviour derived class as a component on an object. 
    /// In this way, it can be found via <c>GetComponent<></c>.
    /// </summary>
    /// <typeparam name="T">The type of object that will be passed in and displayed.</typeparam>
    /// <example>
    /// For an example, see the <see cref="TextDisplay"/> class.
    /// </example>
    public interface IDisplay<T>
    {
        void Display(T obj);
    }


}