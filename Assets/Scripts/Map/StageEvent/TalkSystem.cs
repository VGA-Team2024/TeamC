using UnityEngine;

/// <summary> 会話システム </summary>
public class TalkSystem : MonoBehaviour
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
        
        // ToDo:会話の詳細がきたらシステムを作る

    }

    private void ShowBackgroundPanel()
    {
        _panel.SetActive(true);
    }
}
