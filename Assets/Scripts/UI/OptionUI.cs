using UnityEngine;

public class OptionUI : MonoBehaviour
{
    [SerializeField, InspectorVariantName("操作説明ボタン")] 
    private UIButton _manualButton;

    [SerializeField] 
    private GameObject _manualUI;

    [SerializeField, InspectorVariantName("音量ボタン")]
    private UIButton _volumeButton;
    
    [SerializeField]
    private GameObject _volumeUI;
    
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _manualButton.OnClickAddListener(() => SetManualPanel(true));
        _volumeButton.OnClickAddListener(() => SetVolume(true));
    }
    
    private void SetManualPanel(bool isOn)
    {
        _manualUI.gameObject.SetActive(isOn);
    }

    private void SetVolume(bool isOn)
    {
        _volumeUI.gameObject.SetActive(isOn);
    }
}
