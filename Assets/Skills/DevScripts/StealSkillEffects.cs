using UnityEngine;

namespace TeamC
{
    /// <summary>スティールの効果</summary>
    [CreateAssetMenu(fileName = "GeneratedStealSkillEffects", menuName = "CreateSkillEffects/CreateStealSkillEffects")]
    public class StealSkillEffects : ScriptableObject
    {
        private StealSkill _stealSkill;
        private Boss _boss;
        private Player _player;

        /// <summary>スティールの効果発動時の処理</summary>
        public void OnDestroySkillEffects()
        {
            if (_stealSkill == null)
                _stealSkill = FindFirstObjectByType<StealSkill>();
            if (_boss == null)
                _boss = FindFirstObjectByType<Boss>();
            if (_player == null)
                _player = FindFirstObjectByType<Player>();
            
            if (_stealSkill.GetIsLocked())
            {
#if UNITY_EDITOR
                Debug.Log("スキルロック中です。");
#endif
                return;
            }

            if (_stealSkill.GetIsCoolTime())
            {
#if UNITY_EDITOR
                Debug.Log("クールタイム中です。");
#endif
                return;
            }

            // ボスを撃破したときのゴールドの1/10を獲得できます。
            decimal rewards = _boss.GetReward() / 10;
            _player.ApplyRewardToPlayer(rewards);
            
#if UNITY_EDITOR
            Debug.Log("スティール!!");
#endif
        }
    }
}
