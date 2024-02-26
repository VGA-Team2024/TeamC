using UnityEngine;

namespace TeamC
{
    /// <summary>スティールの効果</summary>
    [CreateAssetMenu(fileName = "GeneratedStealEffects", menuName = "CreateStealEffects")]
    public class StealEffects : ScriptableObject
    {
        private Steal _steal;
        private BossLagacy _bossLagacy;
        private PlayerL _playerL;

        /// <summary>スティールの効果発動時の処理</summary>
        public void OnDestroySkillEffects()
        {
            if (_steal == null)
                _steal = FindFirstObjectByType<Steal>();
            if (_bossLagacy == null)
                _bossLagacy = FindFirstObjectByType<BossLagacy>();
            if (_playerL == null)
                _playerL = FindFirstObjectByType<PlayerL>();
            
            if (_steal.GetIsLocked())
            {
#if UNITY_EDITOR
                Debug.Log("スキルロック中です。");
#endif
                return;
            }

            if (_steal.GetIsCoolTime())
            {
#if UNITY_EDITOR
                Debug.Log("クールタイム中です。");
#endif
                return;
            }

            // ボスを撃破したときのゴールドの1/10を獲得できます。
            decimal rewards = _bossLagacy.GetReward() / 10;
            _playerL.ApplyRewardToPlayer(rewards);
            
#if UNITY_EDITOR
            Debug.Log("スティール!!");
#endif
        }
    }
}
