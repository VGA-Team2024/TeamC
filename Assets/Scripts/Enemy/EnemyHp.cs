using UnityEngine;

public class EnemyHp : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHp;
    private int _currentHp;
    public int CurrentHp => _currentHp;
    
    private void Start()
    {
        _currentHp = _maxHp;
    }

    private void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
    }
}
