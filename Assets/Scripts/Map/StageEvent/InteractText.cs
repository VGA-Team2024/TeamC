using UniRx;
using UnityEngine;

/// <summary> トリガーに入ったときにテキストを表示する </summary>
public class InteractText : MonoBehaviour
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
                ShowInteractText();
            }
        }).AddTo(this);

        _triggerEvent.OnTriggerExitAsObservable.Subscribe(collider =>
        {
            if (collider == _collider)
            {
                HideInteractText();
            }
        }).AddTo(this);
    }

    private void ShowInteractText()
    {
        _interactText.SetActive(true);
    }

    private void HideInteractText()
    {
        for (var i = 0; i < _interactText.transform.childCount; i++)
        {
            _interactText.transform.GetChild(i).gameObject.SetActive(false);
        }
        _interactText.SetActive(false);
    }
}
