using UnityEngine;

/// <summary> BGMの再生を行う </summary>
public class BGMManager : MonoBehaviour
{
    [SerializeField, Header("BGMのキューシート名")] private string _bgmCueSheet;
    [SerializeField, Header("森BGMのキュー名")] private string _forestBGM;
    
    private bool _playing;
    
    void Start()
    {
        CRIAudioManager.Initialize();
        CRIAudioManager.BGM.Play(_bgmCueSheet, _forestBGM);
        _playing = true;
    }

    void Update()
    {
        // 仮
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_playing)
            {
                CRIAudioManager.BGM.Stop();
            }
            else
            {
                CRIAudioManager.BGM.Play(_bgmCueSheet, _forestBGM);
            }

            _playing = !_playing;
        }
    }
}
