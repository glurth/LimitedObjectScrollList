using UnityEngine.Events;

namespace EyE.Unity.UI
{
    /// <summary>
    /// Interface for handling selection events.
    /// </summary>
    public interface ITriggerOnSelect
    {
        /// <summary>
        /// Event triggered when the UI element is selected.
        /// </summary>
        /// <remarks>
        /// Implement this interface if you want the user to be able to respond to selection events on your UI element.
        /// </remarks>
        UnityEvent onSelectEvent { get; }
    }

    /// <summary>
    /// Interface for handling hover events.
    /// </summary>
    public interface ITriggerOnHover
    {
        /// <summary>
        /// Event triggered when the pointer enters the UI element.
        /// </summary>
        /// <remarks>
        /// Implement this interface if you want the user to be able to respond to pointer enter events on your UI element.
        /// </remarks>
        UnityEvent onPointerEnterEvent { get; }

        /// <summary>
        /// Event triggered when the pointer exits the UI element.
        /// </summary>
        /// <remarks>
        /// Implement this interface if you want the user to be able to respond to pointer exit events on your UI element.
        /// </remarks>
        UnityEvent onPointerExitEvent { get; }
    }

    /// <summary>
    /// Interface for handling click events.
    /// </summary>
    public interface ITriggerOnClick
    {
        /// <summary>
        /// Event triggered when the UI element is clicked.
        /// </summary>
        /// <remarks>
        /// Implement this interface if you want the user to be able to respond to click events on your UI element.
        /// </remarks>
        UnityEvent onClickEvent { get; }
    }

}