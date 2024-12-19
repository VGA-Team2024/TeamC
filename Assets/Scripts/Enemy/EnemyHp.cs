using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField] private int _maxHp;
    private int _currentHp;
    public int CurrentHp => _currentHp;
    
    private void Start()
    {
        _currentHp = _maxHp;
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
    }
}
