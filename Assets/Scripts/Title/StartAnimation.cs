using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Title
{
    /// <summary>タイトルで流す演出を管理する </summary>
    public class StartAnimation : MonoBehaviour
    {
        #region アタッチが必要なオブジェクト

        [SerializeField] private Fade _fade;
        [SerializeField] private TextMeshProUGUI _voiceTextUI;

        #endregion
        
        #region Editorで制御するための変数
        
        [SerializeField, InspectorVariantName("セリフの順番")] private string[] _voiceQueName;
        [SerializeField,InspectorVariantName("セリフの字幕")] private string[] _voiceText;
        [SerializeField,InspectorVariantName("最初に落としたいオブジェクトを登録")] private FirstFallAnimation[] _openingCharacterFirstAnimation;
        [SerializeField,InspectorVariantName("2番目に落としたいオブジェクトを登録")] private SecondFallAnimation[] _openingCharacterSecondAnimation;
        [SerializeField,Header("フェードアウト完了するまでの時間")] private float _fadeTime;
        [SerializeField, InspectorVariantName("フェードアウトした後セリフが始まる時間")] private float _fadeoutDelayTime;
        [SerializeField, InspectorVariantName("テキストを表示する間隔")] private float _textDuration;

        #endregion

        // Animationの組み立て
        public async UniTask TitleAnimation()
        {
            // 木箱を閉める音がでる
            CRIAudioManager.SE.Play("", "");
            _fade.Fadeout(_fadeTime);
            await UniTask.Delay(TimeSpan.FromSeconds(_fadeoutDelayTime));
            await UniTask.WhenAll(PlayVoiceSequence(), PlayAnimation());
        }

        // Voiceとtextを一定間隔で実行する
        private async UniTask PlayVoiceSequence()
        {
            if (_voiceQueName != null && _voiceText != null)
            {
                for (int i = 0; i < _voiceText.Length; i++)
                {
                    PlayVoice(i);
                    await UniTask.Delay(TimeSpan.FromSeconds(_textDuration));
                    Debug.Log($"{i}番目のセリフが再生されました。");
                }
            }
        }
        
        // セリフとテキストを同期して表示
        private void PlayVoice(int index)
        {
            // TODO : ボイスを実装
            //CRIAudioManager.VOICE.Play("", _voiceQueName[index]);
            _voiceTextUI.text = _voiceText[index];
        }
        
        private async UniTask PlayAnimation()
        {
            // 最初に落としたい人形のアニメーションを再生する
            await UniTask.WhenAll(_openingCharacterFirstAnimation.Select(character => character.AnimationSettings()));
            
            // 2番目に落としたい人形のアニメーションを再生する
            await UniTask.WhenAll(_openingCharacterSecondAnimation.Select(character => character.AnimationSettings()));
        }
        
    }
}