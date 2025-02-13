using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent (typeof(Selectable))]
/// <summary>ButtonUIにアタッチするクラス </summary>
public class UIButton : MonoBehaviour, ISubmitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _pressedColor = Color.gray;

    private Action _onClickCallback;
    private Color _startColor;
    private Image _image;
    private Text _buttonText;

    protected void Awake()
    {
        if (TryGetComponent(out _image))
        {
            _startColor = _image.color;
        }
        if (TryGetComponent(out _buttonText))
        {
            _startColor = _buttonText.color;
        }
    }
    public void SetText(string text)
    {
        _text.text = text;
    }
    
    public void OnClickAddListener(Action action)
    {
        _onClickCallback += action;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        _onClickCallback?.Invoke();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _onClickCallback?.Invoke();
    }


    public void OnSelect(BaseEventData eventData)
    {
        if (_image)
        {
            _image.color = _pressedColor;
        }
        if (_buttonText)
        {
            _buttonText.color = _pressedColor;
        }
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (_image)
        {
            _image.color = _startColor;
        }
        if (_buttonText)
        {
            _buttonText.color = _startColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

}