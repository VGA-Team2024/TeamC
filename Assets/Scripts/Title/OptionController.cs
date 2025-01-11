using UnityEngine;

/// <summary>Optionボタンの管理</summary>
public class OptionController : MonoBehaviour
{
    #region ボタン設定
    
    [SerializeField, InspectorVariantName("操作説明ボタン")] private UIButton _operationExplanationButton;
    [SerializeField, InspectorVariantName("音量ボタン")] private UIButton _audioVolumeButton;
    
    #endregion
}
