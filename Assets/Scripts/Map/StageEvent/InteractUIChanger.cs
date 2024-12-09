using UnityEngine;
using UnityEngine.InputSystem;

/// <summary> 会話開始時のUI切り替え </summary>
public class InteractUIChanger : MonoBehaviour
{
    [SerializeField, Header("会話時の背景パネル")] private GameObject _panel; // 動かないのでUIでもOK
    
    private PlayerControls _controls;
    private Vector2 _dir; //ActionMapのMoveの値を保存するVector2
    private const float Threshold = 0.1f;
    
    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.InGame.Move.started += ShowBackgroundPanel;
    }

    private void OnDestroy()
    {
        _controls.Dispose();
        _controls.InGame.Move.started -= ShowBackgroundPanel;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void ShowBackgroundPanel(InputAction.CallbackContext callbackContext)
    {
        _dir = callbackContext.ReadValue<Vector2>();
        
        if (_dir.x is < Threshold and > -Threshold && _dir.y > Threshold)
        { 
             _panel.SetActive(true);
             
             // 自身(テキスト)を非表示にする
             gameObject.SetActive(false);         
        }
    }
}
