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
        [SerializeField,InspectorVariantName("クレジットを表示する間隔")] private float[] _fadeCreditDuration;
        [SerializeField, InspectorVariantName("流したいBGM")] private string _bgmName;
        [SerializeField, InspectorVariantName("Credit再生時に流したいBGM")] private string _creditBgmName;
        [SerializeField, InspectorVariantName("表示させたい一枚絵のリスト")] private Sprite[] _endingSpriteLists;
        [SerializeField, InspectorVariantName("表示させたいクレジットイラストのリスト")] private Sprite[] _creditSpriteLists;

        [SerializeField, InspectorVariantName("エンディングとクレジットの間")] private float _delay;
        
        [SerializeField] private float _fadeDuration;
        #endregion

        private const int _musicIndex = 3;
        private CRIAudioManager.SoundPlayer _musicBoxPlayer;
        private Credit _credit;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _credit = new Credit();
            _credit.Initialize(_fadeController, _backGround, _creditSpriteLists);
            _musicBoxPlayer = new CRIAudioManager.SoundPlayer(SoundType.BGM);
            _musicBoxPlayer.Setup();
            _musicBoxPlayer.SetVolume(1.0f);
            _musicBoxPlayer.Player.SetFirstBlockIndex(_musicIndex);
        }

        public async UniTask PlayEnding()
        {
            _musicBoxPlayer.Play("MusicBox", _creditBgmName);
            await ShowEndingMovie();
            _musicBoxPlayer.Stop();
            _musicBoxPlayer.Play("BGM", _bgmName);
            await _credit.ShowCredit(_delay,_fadeCreditDuration);
            _musicBoxPlayer.Stop();
            await StartFadeOut();
            SceneLoader.LoadScene("01_Title");
        }

        // オルゴールを渡すを選択すると1枚絵が表示されBGMが流れ始める
        // 一定間隔でイラストがながれていく(4枚)　切り換えるときもFadeで切り換え
        private　async UniTask ShowEndingMovie()
        {
            await StartFadeOut();
            _backGround.gameObject.SetActive(true);
            // 一枚絵を切り替える
            for (int i = 0; i < _endingSpriteLists.Length; i++)
            {
                // Fadeの処理
                if (i != 0)
                {
                    await StartFadeOut();
                }
                
                _backGround.sprite = _endingSpriteLists[i];
                // 表示間隔分待つ
                await UniTask.Delay(TimeSpan.FromSeconds(_endingFadeDuration[i]));
                // Spriteの切り替え
                await StartFadeIn();
            }

            await StartFadeOut();
        }
        
        // 最後に黒い画面へフェードアウト
        private async UniTask StartFadeIn()
        {
            await _fadeController.FadeInAsync(_fadeDuration);
        }

        // 最後に黒い画面へフェードアウト
        private async UniTask StartFadeOut()
        {
            await _fadeController.FadeOutAsync(_fadeDuration);
        }
    }
}