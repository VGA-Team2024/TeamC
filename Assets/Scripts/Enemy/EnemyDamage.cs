using UnityEngine;

public class EnemyDamage : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator _animator;
    private EnemyHp _hp;
    private readonly int _damage = Animator.StringToHash("Damage");

    private void Start()
    {
        _hp = GetComponent<EnemyHp>();
    }

    public void TakeDamage(int damage)
    {
        if (_animator) _animator.Play(_damage);
        _hp.TakeDamage(damage);
    }
}