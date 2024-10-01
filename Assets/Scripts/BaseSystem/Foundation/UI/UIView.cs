using UISystem;
using UnityEngine;

namespace UISystem
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class UIView : MonoBehaviour
    {
        [SerializeField] private bool _isActive;
        [SerializeField] protected string _viewId;

        public bool IsActive { get; protected set; }

        public RectTransform RectTransform { get; protected set; }
        protected CanvasGroup _canvasGroup = null;

        void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_isActive)
            {
                _canvasGroup.alpha = 1.0f;
                IsActive = true;
            }
            else
            {
                _canvasGroup.alpha = 0.0f;
                IsActive = false;
            }
            AwakeCall();
        }

        protected virtual void AwakeCall()
        {

        }

        public virtual void Active()
        {
            _canvasGroup.alpha = 1.0f;
            IsActive = true;
        }

        public virtual void Disactive()
        {
            _canvasGroup.alpha = 0.0f;
            IsActive = false;
        }

        public virtual void Enter()
        {
            Active();
        }

        public virtual void Exit()
        {
            Disactive();
        }

        private void OnEnable()
        {
            if (_viewId == "")
            {
                _viewId = this.name;
            }
        }
    }
}