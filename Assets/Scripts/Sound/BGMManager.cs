using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary> ステージのエリアと対応するBGMを保存する構造体 </summary>
[Serializable]
public class BGMSoundCue
{
    [SerializeField] private StageEnum _stageEnum;
    [SerializeField] private string _cueName;
    public StageEnum StageEnum => _stageEnum;
    public string CueName => _cueName;
}

/// <summary> BGMの再生を行う </summary>
public class BGMManager : MonoBehaviour
{
    [SerializeField, InspectorVariantName("BGMのキューシート(string)")] private string _bgmCueSheet;
    [SerializeField] private List<BGMSoundCue> _soundList;

    private const int DiscardCount = 1; // 初期値を通さないようにするための値
    private int _changeCount;           // BGMを変更した回数
    private StageStateManager _stageStateManager;
    private StageEnum _currentStage;
    private bool _playing;
    
    void Start()
    {
        CRIAudioManager.Initialize();
        _playing = true;

        // ステージ状態の変更を監視してBGMを切り替える
        _stageStateManager = FindObjectOfType<StageStateManager>();
        if (!_stageStateManager)
        {
            Debug.LogError("StageManagerプレハブをシーン上に置いてください");
        }
        
        _stageStateManager.CurrentStageState
            .Skip(DiscardCount)
            .DistinctUntilChanged()
            .Subscribe(newState =>
            {
                ChangeBGM(newState);
            }).AddTo(this);
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
                ChangeBGM(_currentStage);
            }

            _playing = !_playing;
        }
    }
    
    // BGMをステージの状態に合わせて変更する
    private void ChangeBGM(StageEnum newState)
    {
        // BGMが一度でも再生されていたら
        if (_changeCount > 0)
        {
            CRIAudioManager.BGM.Stop();
        }

        for (int i = 0; i < _soundList.Count; i++)
        {
            var data =_soundList[i];
            if (newState == data.StageEnum)
            {
                CRIAudioManager.BGM.Play(_bgmCueSheet, data.CueName);
                _currentStage = newState;
                _changeCount++;
            }
        }
    }
}

