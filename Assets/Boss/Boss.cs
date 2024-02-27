using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Image _img;
        [SerializeField] List<Sprite> _tex;

        int _cfloor = 0;

        Player _p;

        void Start()
        {
            _img = GetComponent<Image>();
            _p = FindFirstObjectByType<Player>();
            base.CallbackOnDeath += ChangeTexture;
        }

        void ChangeTexture()
        {
            _cfloor = _p.GetClearedFloorAmount();
            if (_cfloor >= 0 && _cfloor < 6)
                _img.sprite = _tex[0];
            else if (_cfloor < 11)
                _img.sprite = _tex[1];
            else if (_cfloor < 16)
                _img.sprite = _tex[2];
            else if (_cfloor < 21)
                _img.sprite = _tex[3];
            else if (_cfloor <= 26)
                _img.sprite = _tex[4];
        }

        void Update()
        {
            if (IsBossDead())
            {
                base.OnDeath();
            }
        }

        bool IsBossDead()
        {
            return base.GetHP <= 0;
        }
    }
}
