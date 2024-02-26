using UnityEngine;

namespace TeamC
{
    /// <summary>スマッシュの効果</summary>
    [CreateAssetMenu(fileName = "GeneratedSmashSkillEffects", menuName = "CreateSkillEffects/CreateSmashSkillEffects")]
    public class SmashSkillEffects : ScriptableObject
    {
        private SmashSkill _smashSkill;
        private Boss _boss;
        private Player _player;
        
        /// <summary>スマッシュの効果発動時の処理</summary>
        public void OnDestroySkillEffects()
        {
            if (_smashSkill == null)
                _smashSkill = FindFirstObjectByType<SmashSkill>();
            if (_boss == null)
                _boss = FindFirstObjectByType<Boss>();
            if (_player == null)
                _player = FindFirstObjectByType<Player>();
            
            if (_smashSkill.GetIsLocked())
            {
#if UNITY_EDITOR
                Debug.Log("スキルロック中です。");
#endif
                return;
            }

            if (_smashSkill.GetIsCoolTime())
            {
#if UNITY_EDITOR
                Debug.Log("クールタイム中です。");
#endif
                return;
            }

            // プレイヤーの攻撃力の10倍のダメージを与えます
            decimal damage = _player.GetPlayerApplayingDamage * 10;
            _boss.ApplyDamageToBoss(damage);
            
#if UNITY_EDITOR
            Debug.Log("スマッシュ!!");
#endif
        }
    }
}
