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
        [SerializeField] private GameObject _chapterPanel;
        [SerializeField] private GameObject _optionsPanel;

        private async void Start()
        {
            await _movieAnimation.PrepareMovie();
            CRIAudioManager.Initialize();
            Initialize();
        }

        // 初期化
        private void Initialize()
        {
            _startButton.OnClickAddListener(OnClickStartButton);
            _optionButton.OnClickAddListener(OnClickOptionButton);
        }
        
        // ボタン押下時の処理
        private void OnClickStartButton()
        {
            _startButton.gameObject.SetActive(false);
            _optionButton.gameObject.SetActive(false);
            _chapterPanel.SetActive(true);
        }

        private void OnClickOptionButton()
        {
            _optionsPanel.SetActive(true);
        }
    }
}