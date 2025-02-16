using UnityEngine;

public class OptionUI : MonoBehaviour
{
    [SerializeField, InspectorVariantName("操作説明ボタン")] private UIButton _manualButton;

    [SerializeField] private GameObject _manualUI;

    [SerializeField, InspectorVariantName("音量ボタン")] private UIButton _volumeButton;
    
    [SerializeField] private GameObject _volumeUI;

    [SerializeField] private PlayerMove _player;
    [SerializeField] private GameObject _optionUI;
    
    [SerializeField] private UIButton _backButton;
    
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _manualButton.OnClickAddListener(() => SetManualPanel(true));
        _volumeButton.OnClickAddListener(() => SetVolume(true));
        _backButton.OnClickAddListener(OnClickBackButton);
        _player.IsFreeze = (true,true);
    }

    private void OnClickBackButton()
    {
        _player.IsFreeze = (false,false);
    }
    
    private void SetManualPanel(bool isOn)
    {
        _manualUI.gameObject.SetActive(isOn);
        _optionUI.gameObject.SetActive(!isOn);
    }

    private void SetVolume(bool isOn)
    {
        _volumeUI.gameObject.SetActive(isOn);
    }
}
