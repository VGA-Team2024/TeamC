using UnityEngine;
using UnityEngine.Video;

namespace Title
{
    /// <summary>Titleシーンの管理</summary>
    public class OpeningStoryTitleController : MonoBehaviour
    {
        [SerializeField] private UIButton _startButton;
        [SerializeField] private VideoPlayer _videoPlayer;

        private void Start()
        {
            Initialize();
        }

        // 初期化
        private void Initialize()
        {
            _startButton.OnClickAddListener(HandleStartButtonClick);
        }
        
        // ボタン押下時の非同期処理
        private void HandleStartButtonClick()
        {
            _startButton.gameObject.SetActive(false);
            _videoPlayer.gameObject.SetActive(true);
        }
    }
}