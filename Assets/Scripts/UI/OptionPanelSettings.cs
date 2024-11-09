using UnityEngine;

public class OptionPanelSettings : MonoBehaviour
{
    [SerializeField] private UIButton _bgmSettingButton;
    [SerializeField] private UIButton _inGameBackButton;
    [SerializeField] private UIButton _resultBackButton;
    [SerializeField,Header("非アクティブ状態のPanelを入れる")] private GameObject[] _panelList;

    private void Start()
    {
        _bgmSettingButton.OnClickAddListener(OnClickBgmVolumeSetting);
        _inGameBackButton.OnClickAddListener(OnClickCloseOptionPanel);
        _resultBackButton.OnClickAddListener(OnClickResultBackButton);
    }

    // BGMVolumeの設定画面を開く
    private void OnClickBgmVolumeSetting()
    {
        
    }
    
    // この画面を閉じる
    private void OnClickCloseOptionPanel()
    {
        
    }
    
    // タイトル画面に戻る
    private void OnClickResultBackButton()
    {
        
    }
}