using System;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public Animator Animator => _animator;
    
    [SerializeField] private PlayerAnimationEventController _animEvent;
    public PlayerAnimationEventController AnimEvent => _animEvent;
    
    [SerializeField] private PlayerStatusUI _playerStatusUI;
    public PlayerStatusUI PlayerStatusUI => _playerStatusUI;

    public Rigidbody Rigidbody { get; private set; }
    
    public PlayerMove PlayerMove {get; private set; }
    
    public PlayerSounds PlayerSounds { get; private set; }
    
    public PlayerStatus PlayerStatus { get; private set; }
    
    public PlayerAttack PlayerAttack { get; private set; }
    
    public PlayerMusicBox PlayerMusicBox { get; private set; }
    
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    
    public CancellationToken CancellationToken => _cancellationTokenSource.Token;

    public GameOverManager GameOver {get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        PlayerMove = GetComponent<PlayerMove>();
        PlayerSounds = GetComponent<PlayerSounds>();
        PlayerStatus = GetComponent<PlayerStatus>();
        PlayerAttack = GetComponent<PlayerAttack>();
        PlayerMusicBox = GetComponent<PlayerMusicBox>();
    }

    private void Start()
    {
        try
        {
            _playerStatusUI= GameObject.Find("PlayerUICanvas").GetComponent<PlayerStatusUI>();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"PlayerUICanvas can't be found \n {e.Message}");
        }

        GameOverManager.I.Player = this;
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }
}
