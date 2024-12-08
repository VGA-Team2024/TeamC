using System;
using System.Linq;
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

        #region Editorで制御する変数

        private string[] _voiceText;
        private float _fadeTime;
        private float _fadeoutDelayTime;
        private float _textDuration;
        private float _orgelFadeoutTime;
        private float _orgelFadePosition;

        [SerializeField] private float[] _textDurations;

        #endregion

        [SerializeField, InspectorVariantName("セリフの順番")]
        private string[] _voiceQueName;

        [SerializeField, InspectorVariantName("最初に落としたいオブジェクトを登録")]
        private FallAnimation[] _openingCharacterFirstAnimation;

        [SerializeField, InspectorVariantName("2番目に落としたいオブジェクトを登録")]
        private FallAnimation[] _openingCharacterSecondAnimation;

        [SerializeField, InspectorVariantName("オルゴールを持ち去る演出時に出したいオブジェクトを登録")]
        private EnemySnatchAnimation[] _snatchCharacterAnimation;

        [SerializeField, InspectorVariantName("オルゴールオブジェクト")]
        private OrgelAnimation _orgel;

        private Vector3 _targetPosition;

        public EditorDebugSo EditorDebugSo;
        
        [SerializeField] private AudioSource _audioSource;

        private void Awake()
        {
            _voiceText = new string[EditorDebugSo.VoiceText.Length];
            for (int i = 0; i < EditorDebugSo.VoiceText.Length; i++)
            {
                _voiceText[i] = EditorDebugSo.VoiceText[i];
            }

            _fadeTime = EditorDebugSo.FadeTime;
            _fadeoutDelayTime = EditorDebugSo.FadeoutDelayTime;
            _textDuration = EditorDebugSo.TextDuration;
            _orgelFadeoutTime = EditorDebugSo.OrgelFadeoutTime;
            _targetPosition = _snatchCharacterAnimation[1].GetCurrentPosition();
            _orgelFadePosition = EditorDebugSo.OrgelFadePosition;
        }

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
            // ToDo : VoiceがCRI実装されたら削除
            _audioSource.clip = EditorDebugSo._voiceClip;
            _audioSource.Play();
            
            if (_voiceText != null)
            {
                //　セリフとDurationの数が一致していなかった場合は少ないほうに合わせる
                int length = Math.Min(_voiceText.Length, _textDurations.Length);
                for (int i = 0; i < length; i++)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_textDurations[i]));
                    PlayVoice(i);
                }
            }
            else
                Debug.LogError("字幕設定がされていません");
        }

        // 字幕の表示
        private void PlayVoice(int index)
        {
            _voiceTextUI.text = _voiceText[index];
        }

        // TODO : キャンセルトークンを設定
        private async UniTask PlayAnimation()
        {
            // 最初に落としたい人形のアニメーションを再生する
            await UniTask.WhenAll(
                _openingCharacterFirstAnimation.Select(character => character.FirstAnimationSettings()));

            // 2番目に落としたい人形のアニメーションを再生する
            await UniTask.WhenAll(
                _openingCharacterSecondAnimation.Select(character => character.SecondAnimationAnimationSettings()));

            // 敵がオルゴールのもとにやってくる
            await UniTask.WhenAll(_snatchCharacterAnimation.Select(character => character.Move()));

            // 敵を傾ける
            await UniTask.WhenAll(_snatchCharacterAnimation.Select(character => character.HeelOver()));

            // オルゴールを上に上げる
            await _orgel.OrgelUpAnimation();

            // 小人たちのMoveタスクを配列として生成
            var moveTasks =
                _snatchCharacterAnimation.Select(character => character.Fade());

            // オルゴールのフェードアウトタスク
            var orgelFadeOutTask = _orgel.OrgelFadeOut(_orgelFadePosition);

            // MoveタスクとOrgelFadeOutタスクを並列に実行
            await UniTask.WhenAll(moveTasks.Concat(new[] { orgelFadeOutTask }));
        }
    }
}