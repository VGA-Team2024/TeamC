using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMove _playerMove;
    public PlayerMove PlayerMove => _playerMove;
    
    [SerializeField] Animator _animator;
    public Animator Animator => _animator;
    
    [SerializeField] private SpriteStudioAnimationEventScript _animationEvent;
    public SpriteStudioAnimationEventScript AnimationEvent => _animationEvent;
    
    private PlayerSounds _playerSounds;
    public PlayerSounds PlayerSounds => _playerSounds;
    
    private PlayerStatus _playerStatus;
    public PlayerStatus PlayerStatus => _playerStatus;
    
    private PlayerAttack _playerAttack;
    public PlayerAttack PlayerAttack => _playerAttack;
    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerSounds = GetComponent<PlayerSounds>();
        _playerStatus = GetComponent<PlayerStatus>();
    }
}
