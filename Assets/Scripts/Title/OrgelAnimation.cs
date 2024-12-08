using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    // オルゴールのアニメーションを管理
    public class OrgelAnimation : OpeningCharacterAnimation
    {
        [SerializeField] private Image _orgel;

        #region エディターで編集する変数

        [SerializeField] private float _orgelUpPosition;

        #endregion
        
        public override async UniTask AnimationSettings() { }

        // オルゴールを上にあげるアニメーション
        public async UniTask OrgelUpAnimation()
        {
            // オルゴールがあがる
            await _orgel.transform
                .DOMove(
                    new Vector3(_orgel.transform.position.x, _orgel.transform.position.y + _orgelUpPosition,
                        _orgel.transform.position.z), _duration).SetEase(Ease.InOutQuad);
        }
        
        // オルゴールのフェードアウトアニメーション
        public async UniTask OrgelFadeOut(float x)
        {
            await _orgel.transform.DOMove(new Vector3(_orgel.transform.position.x + x, _orgel.transform.position.y, _orgel.transform.position.z), _duration);
        }
    }
}