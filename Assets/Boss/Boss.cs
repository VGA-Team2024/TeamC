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
        void Update()
        {
            if (IsDeadBoss())
                base.OnDeath();
        }
        bool IsDeadBoss()
        {
            return base.GetHP <= 0;
        }
    }
}