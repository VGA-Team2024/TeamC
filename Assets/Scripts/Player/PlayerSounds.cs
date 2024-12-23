using System;
using System.Collections.Generic;
using CriWare;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private string _playerCueSheet;
    [SerializeField] private string _musicBoxCueSheet;
    [SerializeField] private List<SoundCue> soundList;
    private Player _player;
    private CRIAudioManager.SoundPlayer _musicBoxPlayer;

    public CRIAudioManager.SoundPlayer MusicBoxPlayer => _musicBoxPlayer;


    public CriAtomExPlayback PlayerSEPlay(PlayerSoundEnum sound)
    {
        CriAtomExPlayback playback = default;
        foreach (SoundCue s in soundList)
        {
            if (sound == s.SoundEnum)
            {
                if (sound == PlayerSoundEnum.MusicBox)
                {
                    playback = _musicBoxPlayer.Play(_musicBoxCueSheet, s.Name);
                }
                else
                {
                    playback = CRIAudioManager.SE.Play(_playerCueSheet, s.Name);
                }
            }
        }
        return playback;
    }
    private void Start()
    {
        _player = GetComponent<Player>();
        
        // オルゴール用のサウンドプレイヤー作成と設定
        _musicBoxPlayer = new CRIAudioManager.SoundPlayer(SoundType.BGM);
        _musicBoxPlayer.Setup();
        _musicBoxPlayer.SetVolume(1.0f);
        
        void StepPlay() => PlayerSEPlay(PlayerSoundEnum.FootSteps);
        _player.AnimEvent.AnimEventDic.Add(PlayerAnimationEventController.animationType.StepAudio,StepPlay);
    }
}


[Serializable]
public class SoundCue
{
    public PlayerSoundEnum SoundEnum;
    public string Name;
}

public enum PlayerSoundEnum
{
    Jump,
    JumpLandhing,
    Attack,
    AttackHit,
    ThrowNeedle,
    FootSteps,
    MusicBox
}