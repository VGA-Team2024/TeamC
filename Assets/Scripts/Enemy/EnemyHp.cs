using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField] private int _maxHp;
    private int _currentHp;
    public int CurrentHp => _currentHp;
    
    private void Start()
    {
        _currentHp = _maxHp;
    } // StartではなくOnEnableでないとCurrentHpの初期化が行われない

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
    }
}
