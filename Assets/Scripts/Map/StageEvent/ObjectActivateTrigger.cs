using UniRx;
using UnityEngine;

/// <summary> OnTriggerEventを付けた対象がトリガーに入ったときにオブジェクトを表示する </summary>
public class ObjectActivateTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _interactText;
    
    private OnTriggerEvent _triggerEvent;
    private Collider _collider;
    
    private void Start()
    {
        _triggerEvent = FindObjectOfType<OnTriggerEvent>();
        _collider = GetComponent<Collider>();
        
        _triggerEvent.OnTriggerEnterAsObservable.Subscribe(collider =>
        {
            if (collider == _collider)
            {
                ShowObject();
            }
        }).AddTo(this);

        _triggerEvent.OnTriggerExitAsObservable.Subscribe(collider =>
        {
            if (collider == _collider)
            {
                HideObject();
            }
        }).AddTo(this);
    }

    private void ShowObject()
    {
        _interactText.SetActive(true);
    }

    private void HideObject()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
