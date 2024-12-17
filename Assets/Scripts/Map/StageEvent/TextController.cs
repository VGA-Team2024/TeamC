using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary> 会話テキスト表示のためのスクリプト </summary>
public class TextController : MonoBehaviour
{
    [SerializeField, InspectorVariantName("1文字表示ごとの間隔")] private float _textSpeed = 0.15f;
    [SerializeField, InspectorVariantName("セリフを表示するテキスト")] private Text _textLabel;
    [SerializeField, InspectorVariantName("名前を表示するテキスト")] private Text _nameLabel;
    [SerializeField, TextArea(1, 4), Header("セリフ(1度の表示で20文字4行が限界)")] private string[] wards;
    [SerializeField, InspectorVariantName("キャラクター名")] private string _name;
    [SerializeField, InspectorVariantName("名前を表示するかどうか")] private bool _foundName;

    private PlayerMove _player;
    private PlayerControls _controls;
    private CancellationTokenSource _cts;
    private CancellationToken _token;
    private bool _hasTextEnded;  // 一文表示終了フラグ
    private bool _isPressed;

    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.InGame.Jump.started += OnPress;
        _controls.InGame.Jump.canceled += OnRelease;
    }

    private void OnDestroy()
    {
        _controls.Dispose();
        _controls.InGame.Jump.started -= OnPress;
        _controls.InGame.Jump.canceled -= OnRelease;
    }

    private async void OnEnable()
    {
        // プレイヤーの動きを制限
        _player = FindObjectOfType<PlayerMove>();
        _player.IsFreeze = (true, true);
        
        _controls.Enable();
        _cts = new CancellationTokenSource();
        _token = _cts.Token;
        
        await DisplayAllTexts(_token);
    }

    // 会話途中でキャンセルしたとき
    private void OnDisable()
    {
        TalkEnd();
        _controls.Disable();
    }

    void Update()
    {
        if (_foundName)
        {
            ShowName();
        }
        else
        {
            HideName();
        }
    }

    private void OnPress(InputAction.CallbackContext callbackContext)
    {
        _isPressed = true;
    }
    private void OnRelease(InputAction.CallbackContext callbackContext)
    {
        _isPressed = false;
    }

    private async UniTask DisplayAllTexts(CancellationToken cancellationToken)
    {
        try
        {
            foreach (var text in wards)
            {
                // 1文表示終了フラグをリセット
                _hasTextEnded = false;
                await ShowText(text, _token);

                // 表示が終わるまで待機
                await UniTask.WaitUntil(() => _hasTextEnded, cancellationToken: cancellationToken);
                // スキップキーが押されるまで待機
                await UniTask.WaitUntil(() => _isPressed, cancellationToken: cancellationToken);
            }
            
            // 全ての会話が終了したとき
            TalkEnd();
        }
        catch (OperationCanceledException)
        {
            // 表示中に移動などでキャンセルしたとき
            _cts.Dispose();
        }
    }
    
    private async UniTask ShowText(string text, CancellationToken cancellationToken)
    {
        try
        {
            //テキストを初期化
            _textLabel.text = "";

            for (int i = 0; i < text.Length; i++)
            {
                _textLabel.text += text[i];

                // 表示間隔をあける
                await UniTask.Delay((int)(_textSpeed * 1000), cancellationToken: cancellationToken);
            }

            // 一文表示終了
            _hasTextEnded = true;
        }
        catch (OperationCanceledException)
        {
            // 表示中に移動などでキャンセルしたとき
            _cts.Dispose();
        }
    }

    private void ShowName()
    {
        _nameLabel.text = _name;
        _nameLabel.gameObject.SetActive(true);
    }

    private void HideName()
    {
        _nameLabel.gameObject.SetActive(false);
    }

    private void TalkEnd()
    {
        _cts.Cancel();
        
        // プレイヤーの移動制限を解除
        _player.IsFreeze = (false, false);
        // テキストをCanvasごと消す
        transform.parent.gameObject.SetActive(false);
    }
}
