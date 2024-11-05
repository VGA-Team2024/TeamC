using UnityEngine;

/// <summary> 会話開始時のUI切り替え </summary>
public class InteractUIChanger : MonoBehaviour
{
    [SerializeField, Header("インタラクトキー")] private KeyCode _key;
    [SerializeField, Header("会話時の背景パネル")] private GameObject _panel; // 動かないのでUIでもOK
    
    private void Update()
    {
        // インタラクトキーが押されたとき
        if (Input.GetKeyDown(_key))
        {
            // 背景パネルをアクティブにする
            ShowBackgroundPanel();
        }
        
        // ToDo:会話の詳細がきたらシステムを作る(パネル側に別のクラスを作る)

    }

    private void ShowBackgroundPanel()
    {
        _panel.SetActive(true);
        // 自身(テキスト)を非表示にする
        gameObject.SetActive(false);
    }
}
