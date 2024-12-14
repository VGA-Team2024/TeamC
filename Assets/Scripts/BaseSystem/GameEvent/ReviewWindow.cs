using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

/// <summary>
/// レビューダイアログ
/// </summary>
public class ReviewWindow : MonoBehaviour
{
    [SerializeField] InputField _textField;
    [SerializeField] Button _sendButton;

    [SerializeField] ReviewStar[] _starts;
    Action<int, string> _sendFunction;
    Action _sendCallback;
    int _currentStar = 0;


    void Setup()
    {
        //_starts = GetComponentsInChildren<ReviewStar>();
        _sendButton.enabled = false;
        foreach (var s in _starts)
        {
            int index = s.Index;
            s.Setup();
            var evt = s.gameObject.AddComponent<EventTrigger>();

            /*
            var hover = new EventTrigger.Entry();
            hover.eventID = EventTriggerType.PointerEnter;
            hover.callback.AddListener((d) => SetStar(index));
            evt.triggers.Add(hover);
            */

            Button btn = s.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                SetStar(index);
            });
        }
    }

    void SetStar(int index)
    {
        foreach (var s in _starts)
        {
            s.SetStar(s.Index <= index);
        }
        _currentStar = index;

        UpdateSendButton();
    }

    void UpdateSendButton()
    {
        if (_currentStar > 0 && !_sendButton.enabled)
        {
            _sendButton.enabled = true;
            _sendButton.onClick.AddListener(SendReview);
        }
    }

    void SendReview()
    {
        int starNum = _currentStar;
        string reviewtext = _textField.text;
        _sendFunction?.Invoke(starNum, reviewtext);
        _sendCallback?.Invoke();
    }


    //static

    static public ReviewWindow Build(Action<int, string> send, Action callback)
    {
        var obj = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Review/Review.prefab").WaitForCompletion();
        var newObj = Utility.Instantiate(obj);
        Addressables.Release(obj);
        ReviewWindow review = newObj.GetComponent<ReviewWindow>();
        review._sendFunction = send;
        review._sendCallback = callback;
        review.Setup();

        return review;
    }
}