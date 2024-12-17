using System;
using CriWare;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMusicBox : MonoBehaviour , ICylinderAddable
{
    private CriAtomExPlayback _musicBoxPlayback;
    private PlayerControls _controls;
    private Player _player;
    private int _cylinders;
    public int CylinderCount => _cylinders;


    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void Start()
    {
        _player = GetComponent<Player>();
        _controls.InGame.MusicBox.performed += MusicPlay;
        _controls.InGame.MusicBox.canceled += MusicStop;
    }

    private void OnDestroy()
    {
        _controls.InGame.MusicBox.performed -= MusicPlay;
        _controls.InGame.MusicBox.canceled -= MusicStop;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    void MusicPlay(InputAction.CallbackContext context)
    {
        _player.PlayerAttack.MusicBoxPlayingSet = true;
        // ToDo オルゴールが追加されたら鳴らす
        //_musicBoxPlayback = _player.PlayerSounds.PlayerSEPlay(PlayerSoundEnum.MusicBox);
        PlayerEffectManager.Instance.PlayEffect(PlayEffectName.PlayerMusicNoteEffect,0);
        Debug.Log("a");
    }

    void MusicStop(InputAction.CallbackContext context)
    {
        _musicBoxPlayback.Stop();
        PlayerEffectManager.Instance.PlayEffect(PlayEffectName.PlayerMusicNoteEffect,0);
    }
    
    public void AddCylinder()
    {
        _cylinders++;
    }
}
