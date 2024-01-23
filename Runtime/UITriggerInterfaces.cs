using UnityEngine.Events;

namespace EyE.Unity.UI
{

    public interface ITriggerOnSelect
    {
        public UnityEvent onSelectEvent { get; }
    }

    public interface ITriggerOnHover
    {
        public UnityEvent onPointerEnterEvent { get; }
        public UnityEvent onPointerExitEvent { get; }
    }

    public interface ITriggerOnClick
    {
        public UnityEvent onClickEvent { get; }
    }
}