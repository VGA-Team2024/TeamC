using UnityEngine;

namespace TeamC
{
    /// <summary>デストロイの効果</summary>
    [CreateAssetMenu(fileName = "GeneratedDestroySkillEffects", menuName = "CreateSkillEffects/CreateDestroySkillEffects")]
    public class DestroySkillEffects : ScriptableObject
    {
        private DestroySkill _destroySkill;
        private Boss _boss;

        /// <summary>デストロイの効果発動時の処理</summary>
        public void OnDestroySkillEffects()
        {
            if (_destroySkill == null)
                _destroySkill = FindFirstObjectByType<DestroySkill>();
            if (_boss == null)
                _boss = FindFirstObjectByType<Boss>();
            
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
            _boss.ApplyDamageToBoss(_boss.GetHP);

#if UNITY_EDITOR
            Debug.Log("デストロイ!!");
#endif
        }
    }
}
