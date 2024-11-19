using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private string _playerCueSheet;
    [SerializeField] private List<SoundCue> soundList;
    Player _player;
    
    public void PlayerSEPlay(PlayerSoundEnum sound)
    {
        foreach (SoundCue s in soundList)
        {
            if (sound == s.SoundEnum)
            {
                CRIAudioManager.BGM.Play(_playerCueSheet, s.Name);
            }
        }
    }

    private void Start()
    {
        _player = GetComponent<Player>();
        void StepPlay() => PlayerSEPlay(PlayerSoundEnum.FootSteps);
        _player.AnimationEvent.EventDictionary.Add("Step", StepPlay);
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
    FootSteps
}