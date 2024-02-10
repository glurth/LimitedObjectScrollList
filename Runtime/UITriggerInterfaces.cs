using UnityEngine.Events;

namespace EyE.Unity.UI
{
    /// These interfaces may be used by any class, and are specifically used by the LimitedObjectScrollList class.

    /// <summary>
    /// Interface for objects that trigger selection events.
    /// </summary>
    public interface ITriggerOnSelect
    {
        /// <summary>
        /// Event triggered when the UI element is selected.
        /// </summary>
        /// <remarks>
        /// Implement this interface if you want the user of your class to be able to respond to selection events on your UI element.
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
        /// Implement this interface if you want the user of your class to be able to respond to pointer enter events on your UI element.
        /// </remarks>
        UnityEvent onPointerEnterEvent { get; }

        /// <summary>
        /// Event triggered when the pointer exits the UI element.
        /// </summary>
        /// <remarks>
        /// Implement this interface if you want the user of your class to be able to respond to pointer exit events on your UI element.
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
        /// Implement this interface if you want the user of your class to be able to respond to click events on your UI element.
        /// </remarks>
        UnityEvent onClickEvent { get; }
    }

    /// <summary>
    /// Interface for handling value changed events.
    /// </summary>
    public interface ITriggerOnValueChange<T>
    {
        /// <summary>
        /// Event triggered when the UI element's value is changed.
        /// </summary>
        /// <remarks>
        /// Implement this interface if you want the user of your class to be able to respond to valueChanged events on your UI element.
        /// </remarks>
        UnityEvent<T> onValueChanged { get; }
    }

    /// <summary>
    /// Interface for handling value edit-complete events.
    /// </summary>
    public interface ITriggerOnValueEditEnd<T>
    {
        /// <summary>
        /// Event triggered when the UI element's value is done being edited by the user (click away, enter, tab, arrow, etc..).
        /// </summary>
        /// <remarks>
        /// Implement this interface if you want the user of your class to be able to respond to valueChanged events on your UI element.
        /// </remarks>
        UnityEvent<T> onValueEditEnd { get; }
    }

    /// <summary>
    /// Interface for handling value validate events.
    /// </summary>
    public interface ITriggerOnValueValidate<T>
    {
        /// <summary>
        /// Event triggered when the UI element's value is done being edited by the user (click away, enter, tab, arrow, etc..).
        /// </summary>
        /// <remarks>
        /// Implement this interface if you want the user of your class to be able to respond to valueChanged events on your UI element.
        /// </remarks>
        UnityEvent<T> onValueValidate { get; }
    }
}