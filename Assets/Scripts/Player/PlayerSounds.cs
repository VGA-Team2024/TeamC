using System;
using System.Collections.Generic;
using CriWare;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private string _playerCueSheet;
    [SerializeField] private List<SoundCue> soundList;
    private Player _player;
    
    public CriAtomExPlayback PlayerSEPlay(PlayerSoundEnum sound)
    {
        CriAtomExPlayback playback = default;
        foreach (SoundCue s in soundList)
        {
            if (sound == s.SoundEnum)
            {
                playback = CRIAudioManager.BGM.Play(_playerCueSheet, s.Name);
            }
        }
        return playback;
    }
    private void Start()
    {
        _player = GetComponent<Player>();
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