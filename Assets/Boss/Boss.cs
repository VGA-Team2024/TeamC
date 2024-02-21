using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    #region TODO

    // 2/17 菅沼 → 樋口
    // 毎フレームボスの死亡判定をしてください。もし死亡したならbase.OnDeath()を
    // 呼び出してください。また、実装してほしいメソッドがあれば逐次問い合わせてください。
    // hpはbase.HPで取得できます

    #endregion

    /// <summary> Bossのコンポーネント。これがSceneに存在 </summary>
    public class Boss : BossSuperClass
    {
        [SerializeField] SpriteRenderer _defaultDragonTexture;
        [SerializeField] Sprite _FirstDragonTexture;
        [SerializeField] Sprite _SecondDragonTexture;
        [SerializeField] Sprite _ThirdDragonTexture;
        [SerializeField] Sprite _FourthDragonTexture;
        int currentStage = 0;
        PlayerSuperClass player;
        void Start()
        {
            player = FindFirstObjectByType<PlayerSuperClass>();
        }
        void Update()
        {
            if (IsDeadBoss())
                base.OnDeath();

            currentStage = player.GetClearedFloorAmount();
            //if (currentStage > 0 && currentStage < 6)
            //{

            //}
            if (currentStage >= 6 && currentStage < 11)
                _defaultDragonTexture.sprite = _FirstDragonTexture;
            if (currentStage >= 11 && currentStage < 16)
                _defaultDragonTexture.sprite = _SecondDragonTexture;
            if (currentStage >= 16 && currentStage < 21)
                _defaultDragonTexture.sprite = _ThirdDragonTexture;
            if (currentStage >= 21 && currentStage <= 26)
                _defaultDragonTexture.sprite = _FourthDragonTexture;
        }
        bool IsDeadBoss()
        {
            return base.GetHP <= 0;
        }

    }
}