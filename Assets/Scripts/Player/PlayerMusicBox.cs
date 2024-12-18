using System;
using CriWare;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMusicBox : MonoBehaviour , ICylinderAddable
{
    private static readonly int MusicBox = Animator.StringToHash("MusicBox");
    private CriAtomExPlayback _musicBoxPlayback;
    private PlayerControls _controls;
    private Player _player;
    public bool MusicBoxPlaying {get; set; }
    [SerializeField, InspectorVariantName("回復速度")] private float _healSpeed = 2;
    private float _healTimer;
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

    private void Update()
    {
        if(MusicBoxPlaying)_healTimer+=Time.deltaTime;
        if (_healTimer >= _healSpeed)
        {
            _healTimer -= _healSpeed;
            _player.PlayerStatus.Heal(1);
        }
    }

    void MusicPlay(InputAction.CallbackContext context)
    {
        _healTimer = 0;
        MusicBoxPlaying = true;
        // ToDo オルゴールが追加されたら鳴らす
        //_musicBoxPlayback = _player.PlayerSounds.PlayerSEPlay(PlayerSoundEnum.MusicBox);
        PlayerEffectManager.Instance.PlayEffect(PlayEffectName.PlayerMusicNoteEffect,0);
        _player.Animator.SetBool(MusicBox,true);
    }

    void MusicStop(InputAction.CallbackContext context)
    {
        _musicBoxPlayback.Stop();
        PlayerEffectManager.Instance.PlayEffect(PlayEffectName.PlayerMusicNoteEffect,0);
        _player.Animator.SetBool(MusicBox,false);
    }
    
    public void AddCylinder()
    {
        _cylinders++;
    }
}
