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

    protected void Awake()
    {
        _image = GetComponent<Image>();
        _startColor = _image.color;
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
        _image.color = _pressedColor;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        _image.color = _startColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

}