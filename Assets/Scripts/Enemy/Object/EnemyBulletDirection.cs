using UnityEngine;

public class EnemyBulletDirection : MonoBehaviour, IPlayerTarget
{
    private Vector2 _playerPos;

    private void Start()
    {
        Direction();
    }

    private void Direction()
    {
        var dir = _playerPos - (Vector2)transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void GetPlayerPos(Vector2 playerPos)
    {
        _playerPos = playerPos;
    }
}
