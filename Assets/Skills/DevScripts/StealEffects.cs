using UnityEngine;

namespace TeamC
{
    /// <summary>スティールの効果</summary>
    [CreateAssetMenu(fileName = "GeneratedStealEffects", menuName = "CreateStealEffects")]
    public class StealEffects : ScriptableObject
    {
        private Steal _steal;
        private Boss _boss;
        private Player _player;

        /// <summary>スティールの効果発動時の処理</summary>
        public void OnDestroySkillEffects()
        {
            if (_steal == null)
                _steal = FindFirstObjectByType<Steal>();
            if (_boss == null)
                _boss = FindFirstObjectByType<Boss>();
            if (_player == null)
                _player = FindFirstObjectByType<Player>();
            
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
            decimal rewards = _boss.GetReward() / 10;
            _player.ApplyRewardToPlayer(rewards);
            
#if UNITY_EDITOR
            Debug.Log("スティール!!");
#endif
        }
    }
}
