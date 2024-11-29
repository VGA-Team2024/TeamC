using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    [SerializeField, InspectorVariantName("1文字表示ごとの間隔")] private float _textSpeed = 0.15f;
    [SerializeField, InspectorVariantName("次のテキストに切り替えるキー")] private KeyCode _skipKey = KeyCode.Space;
    [SerializeField, InspectorVariantName("セリフを表示するテキスト")] private Text _textLabel;
    [SerializeField, InspectorVariantName("名前を表示するテキスト")] private Text _nameLabel;
    [SerializeField, TextArea(1, 4), Header("セリフ(1度の表示で20文字4行が限界)")] private string[] wards;
    [SerializeField, InspectorVariantName("キャラクター名")] private string _name;
    [SerializeField, InspectorVariantName("名前を表示するかどうか")] private bool _foundName;

    //private bool _isSlipping;   // 全表示中フラグ スキップ用に作ったけど機能してない
    private bool _hasTextEnded;  // 一文表示終了フラグ

    private CancellationTokenSource _cts;
    private CancellationToken _token;
    
    private async void OnEnable()
    {
        _cts = new CancellationTokenSource();
        _token = _cts.Token;
        
        // ToDO:プレイヤーの移動の入力を受け付けないようにする
        
        await DisplayAllTexts(_token);
    }

    // 会話途中で移動したとき(仮)
    private void OnDisable()
    {
        _cts.Cancel();
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

    private async UniTask DisplayAllTexts(CancellationToken cancellationToken)
    {
        try
        {
            foreach (var text in wards)
            {
                _hasTextEnded = false; // 1文表示終了フラグをリセット
                await ShowText(text, _token);

                // 表示が終わるまで待機
                await UniTask.WaitUntil(() => _hasTextEnded, cancellationToken: cancellationToken);

                await UniTask.WaitUntil(() => Input.GetKeyDown(_skipKey), cancellationToken: cancellationToken);
                //cts.Cancel();

            }

            // 話が終わったらテキストをCanvasごと消す
            if (Input.GetKeyDown(_skipKey))
            {
                transform.parent.gameObject.SetActive(false);
            }
        }
        catch (OperationCanceledException)
        {
            // 一文表示終了後に移動したとき(仮)
            _cts.Dispose();
        }
        
    }
    
    private async UniTask ShowText(string text, CancellationToken cancellationToken)
    {
        try
        {
            _textLabel.text = ""; //テキストを初期化
            //_isSlipping = false;  // スキップ状態をリセット

            for (int i = 0; i < text.Length; i++)
            {
                // スキップキーが押されていたとき
                // if (_isSlipping)
                // {
                //     _textLabel.text = text;
                //     break;                
                // }

                _textLabel.text += text[i];


                // 表示間隔をあける
                await UniTask.Delay((int)(_textSpeed * 1000), cancellationToken: cancellationToken);

                // スキップキーを押した時
                // if (Input.GetKeyDown(_skipKey))
                // {
                //     _isSlipping = true;
                // }
            }

            // 一文表示終了
            _hasTextEnded = true;
        }
        catch (OperationCanceledException)
        {
            // 一文表示中に移動したとき(仮)
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
}
