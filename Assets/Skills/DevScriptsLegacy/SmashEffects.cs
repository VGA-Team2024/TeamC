using UnityEngine;

namespace TeamC
{
    /// <summary>スマッシュの効果</summary>
    [CreateAssetMenu(fileName = "GeneratedSmashEffects", menuName = "CreateSmashEffects")]
    public class SmashEffects : ScriptableObject
    {
        private Smash _smash;
        private BossLagacy _bossLagacy;
        private PlayerL _playerL;
        
        /// <summary>スマッシュの効果発動時の処理</summary>
        public void OnDestroySkillEffects()
        {
            if (_smash == null)
                _smash = FindFirstObjectByType<Smash>();
            if (_bossLagacy == null)
                _bossLagacy = FindFirstObjectByType<BossLagacy>();
            if (_playerL == null)
                _playerL = FindFirstObjectByType<PlayerL>();
            
            if (_smash.GetIsLocked())
            {
#if UNITY_EDITOR
                Debug.Log("スキルロック中です。");
#endif
                return;
            }

            if (_smash.GetIsCoolTime())
            {
#if UNITY_EDITOR
                Debug.Log("クールタイム中です。");
#endif
                return;
            }

            // プレイヤーの攻撃力の10倍のダメージを与えます
            decimal damage = _playerL.GetPlayerApplayingDamage * 10;
            _bossLagacy.ApplyDamageToBoss(damage);
            
#if UNITY_EDITOR
            Debug.Log("スマッシュ!!");
#endif
        }
    }
}
