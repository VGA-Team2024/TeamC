using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    // オルゴールのアニメーションを管理
    public class OrgelAnimation : MonoBehaviour
    {
        [SerializeField, InspectorVariantName("何秒かけて移動するかの間隔")] protected float _duration;
        
        [SerializeField] private Image _orgel;
        [SerializeField] private float _orgelDuration;

        #region エディターで編集する変数

        [SerializeField] private float _orgelUpPosition;

        #endregion

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
        public async UniTask OrgelFadeOut(float position)
        {
            await transform.DOMove(new Vector3(transform.position.x + position,transform.position.y,transform.position.z), _orgelDuration).SetEase(Ease.InOutQuad);
        }
        
        protected void OnDestroy()
        {
            DOTween.Kill(transform);
        }
    }
}