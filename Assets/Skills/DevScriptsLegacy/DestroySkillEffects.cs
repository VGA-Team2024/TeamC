using UnityEngine;

namespace TeamC
{
    /// <summary>デストロイの効果</summary>
    [CreateAssetMenu(fileName = "GeneratedDestroySkillEffects", menuName = "CreateDestroySkillEffects")]
    public class DestroySkillEffects : ScriptableObject
    {
        private DestroySkill _destroySkill;
        private BossLagacy _bossLagacy;

        /// <summary>デストロイの効果発動時の処理</summary>
        public void OnDestroySkillEffects()
        {
            if (_destroySkill == null)
                _destroySkill = FindFirstObjectByType<DestroySkill>();
            if (_bossLagacy == null)
                _bossLagacy = FindFirstObjectByType<BossLagacy>();
            
            if (_destroySkill.GetIsLocked())
            {
#if UNITY_EDITOR
                Debug.Log("スキルロック中です。");
#endif
                return;
            }

            if (_destroySkill.GetIsCoolTime())
            {
#if UNITY_EDITOR
                Debug.Log("クールタイム中です。");
#endif
                return;
            }

            // ボスを倒します
            _bossLagacy.ApplyDamageToBoss(_bossLagacy.GetHP);

#if UNITY_EDITOR
            Debug.Log("デストロイ!!");
#endif
        }
    }
}
