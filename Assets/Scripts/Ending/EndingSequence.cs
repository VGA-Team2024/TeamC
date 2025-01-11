using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Ending
{
    public class EndingSequence : MonoBehaviour
    {
        [SerializeField] private FadeController _fadeController;

        [SerializeField, InspectorVariantName("BackGround")] private Image _backGround;

        #region Editorで編集

        [SerializeField, InspectorVariantName("エンディングを表示する間隔")] private float[] _endingFadeDuration;

        [SerializeField, InspectorVariantName("クレジットを表示する間隔")] private float[] _fadeCreditDuration;

        [SerializeField, InspectorVariantName("Ending再生時に流したいBGM")] private string _endingBgmName;

        [SerializeField, InspectorVariantName("Credit再生時に流したいBGM")] private string _creditBgmName;

        [SerializeField, InspectorVariantName("表示させたい一枚絵のリスト")] private Sprite[] _endingSpriteLists;

        [SerializeField, InspectorVariantName("表示させたいクレジットイラストのリスト")] private Sprite[] _creditSpriteLists;

        [SerializeField, InspectorVariantName("エンディングとクレジットの間")] private float _delay;

        [SerializeField, InspectorVariantName("EndingのFadeoutの時間")] private float _fadeoutDuration;

        [SerializeField, InspectorVariantName("EndingのFadeinの時間")] private float _fadeinDuration;

        [SerializeField, InspectorVariantName("クレジット画面のFadeoutの時間")] private float _fadeOutCreditDuration;

        [SerializeField, InspectorVariantName("クレジット画面のFadeinの時間")] private float _fadeInCreditDuration;

        [SerializeField, InspectorVariantName("タイトル遷移前のFade時間")] private float _finalDuration;

        #endregion

        private const int _musicIndex = 3;
        private CRIAudioManager.SoundPlayer _musicBoxPlayer;
        private Credit _credit;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            CRIAudioManager.Initialize();
            _credit = new Credit();
            _credit.Initialize(_fadeController, _fadeOutCreditDuration, _fadeInCreditDuration, _backGround,
                _creditSpriteLists);
            _musicBoxPlayer = new CRIAudioManager.SoundPlayer(SoundType.BGM);
            _musicBoxPlayer.Setup();
            _musicBoxPlayer.SetVolume(1.0f);
            _musicBoxPlayer.Player.SetFirstBlockIndex(_musicIndex);
        }

        public async UniTask PlayEnding()
        {
            _musicBoxPlayer.Play("BGM", _endingBgmName);
            await ShowEndingMovie();
            await UniTask.Delay(TimeSpan.FromSeconds(_delay));
            _musicBoxPlayer.Stop();
            _musicBoxPlayer.Play("MusicBox", _creditBgmName);
            await _credit.ShowCredit(_fadeCreditDuration);
            _musicBoxPlayer.Stop();
            await StartFadeOut(_finalDuration);
            SceneLoader.LoadScene("01_Title");
        }

        // オルゴールを渡すを選択すると1枚絵が表示されBGMが流れ始める
        // 一定間隔でイラストがながれていく(4枚)　切り換えるときもFadeで切り換え
        private　async UniTask ShowEndingMovie()
        {
            // エンディングの始まりにFadeする
            await StartFadeOut(_fadeoutDuration);
            _backGround.gameObject.SetActive(true);
            // 一枚絵を切り替える
            for (int i = 0; i < _endingSpriteLists.Length; i++)
            {
                // Fadeの処理
                if (i != 0)
                {
                    // 始まりでFadeをしているので2回目から
                    await StartFadeOut(_fadeoutDuration);
                }

                _backGround.sprite = _endingSpriteLists[i];
                // 表示間隔分待つ
                await UniTask.Delay(TimeSpan.FromSeconds(_endingFadeDuration[i]));
                // Spriteの切り替え
                await StartFadeIn(_fadeinDuration);
            }

            await StartFadeOut(_fadeoutDuration);
        }

        private async UniTask StartFadeIn(float duration)
        {
            await _fadeController.FadeInAsync(duration);
        }

        private async UniTask StartFadeOut(float duration)
        {
            await _fadeController.FadeOutAsync(duration);
        }
    }
}