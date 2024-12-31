using UniRx;
using UnityEngine;

/// <summary> OnTriggerEventを付けた対象がトリガーに入ったときにオブジェクトを表示する </summary>
public class ObjectActivateTrigger : MonoBehaviour
{
    [SerializeField] private InteractUIChanger _interactText;
    
    private OnTriggerEvent _triggerEvent;
    private Collider _collider;

    private void Start()
    {
        Initialize();
        
        _triggerEvent.OnTriggerEnterAsObservable.Subscribe(col =>
        {
            if (col == _collider)
            {
                ShowObject();
            }
        }).AddTo(this);

        _triggerEvent.OnTriggerExitAsObservable.Subscribe(col =>
        {
            if (col == _collider)
            {
                HideObject();
            }
        }).AddTo(this);
    }

    private void Initialize()
    {
        _triggerEvent = FindObjectOfType<OnTriggerEvent>();
        _collider = GetComponent<Collider>();
    }

    private void ShowObject()
    {
        _interactText.gameObject.SetActive(true);
    }

    private void HideObject()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
