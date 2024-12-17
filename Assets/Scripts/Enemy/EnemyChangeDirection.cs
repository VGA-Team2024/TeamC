using UnityEngine;

public class EnemyChangeDirection : MonoBehaviour, IPlayerTarget
{
    [SerializeField, Header("反転するオブジェクト")] private GameObject _obj;
    [SerializeField, Header("画像を変える場合")] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _right;
    [SerializeField] private Sprite _left;
    private PlayerMove _playerMove;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        if (_playerMove)
        {
            Flip();
            ChangeSprite();
        }
    }

    public void GetPlayerMove(PlayerMove playerMove)
    {
        if (playerMove) _playerMove = playerMove;
    }

    private void Flip()
    {
        if (!_obj) return;
        _obj.transform.eulerAngles = new Vector2(0, _playerMove.transform.position.x > _obj.transform.position.x ? 180 : 0);
    }

    private void ChangeSprite()
    {
        if (!_right || !_left) return;
        _spriteRenderer.sprite = _playerMove.transform.position.x > _obj.transform.position.x ? _right : _left;
    }
}
