using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace Title
{
    /// <summary>Titleシーンの管理</summary>
    public class OpeningStoryTitleController : MonoBehaviour
    {
        [SerializeField] private UIButton _startButton;
        [SerializeField] private UIButton _optionButton;
        [SerializeField] private VideoPlayer _videoPlayer;

        [SerializeField] private MovieAnimation _movieAnimation;
        [SerializeField] private GameObject _optionsPanel;
        private CancellationTokenSource _cts;

        private async void Start()
        {
            await _movieAnimation.PrepareMovie();
            CRIAudioManager.Initialize();
            Initialize();
        }

        // 初期化
        private void Initialize()
        {
            _cts = new CancellationTokenSource();
            _startButton.OnClickAddListener(() => UniTask.Void(async () => await HandleStartButtonClick()));
            _optionButton.OnClickAddListener(OnClickOptionButton);
        }
        
        // ボタン押下時の非同期処理
        private async UniTask HandleStartButtonClick()
        {
            _startButton.gameObject.SetActive(false);
            await _movieAnimation.StartMovieAnimation(_cts.Token);
        }

        private void OnClickOptionButton()
        {
            _optionsPanel.SetActive(true);
        }
    }
}