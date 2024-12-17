using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Title
{
    // タイトル画面で最初に落とされる人形のアニメーション
    public class FallAnimation : MonoBehaviour
    {
        #region フィールド

        [SerializeField, InspectorVariantName("回転速度")] protected float _rotationSpeed;

        [SerializeField, InspectorVariantName("何秒かけて移動するかの間隔")] protected float _duration;

        [SerializeField, InspectorVariantName("移動先")] protected Vector3 _moveOffset;

        [SerializeField, InspectorVariantName("落下を開始する時間")] protected float _fallStartTime;

        #endregion
        
        public async UniTask FirstAnimationSettings()
        {
            // 落下タイミングの制御
            await UniTask.Delay(TimeSpan.FromSeconds(_fallStartTime));

            // 回転
            transform.DORotate(new Vector3(0f, 0f, 360f), 1f / (_rotationSpeed / 360f), RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);

            Vector3 targetPosition = transform.position + _moveOffset;

            // 移動のAnimation
            await transform.DOMove(targetPosition, _duration).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                if (this != null)
                    Destroy(gameObject);
            });
        }

        public async UniTask SecondAnimationAnimationSettings()
        {
            // 落下タイミングの制御
            await UniTask.Delay(TimeSpan.FromSeconds(_fallStartTime));

            Vector3 targetPosition = transform.position + _moveOffset;

            // 移動のAnimation
            await transform.DOMove(targetPosition, _duration).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                // 移動が完了したら床にぶつかるアニメーションを発動
                Debug.Log("完了通知");
            });
        }
        
        protected void OnDestroy()
        {
            DOTween.Kill(transform);
        }
    }
}