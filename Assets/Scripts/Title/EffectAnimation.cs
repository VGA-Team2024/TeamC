using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Title
{
    // 妖精のアニメーションを管理する
    public class EffectAnimation : MonoBehaviour
    {
        [Header("妖精のアニメーションを制御")]
        [SerializeField] private AnimationCurve _animationCurveX;
        [SerializeField] private AnimationCurve _animationCurveY;
        [SerializeField] private AnimationCurve _animationCurveZ;

        [SerializeField, InspectorVariantName("目標地点")]
        private GameObject _targetObject;

        [SerializeField, InspectorVariantName("指定した位置に向かうまでの時間")]
        private float _duration;

        private Vector3 _startPosition;

        // 指定の位置まで移動する
        // 呼び出し先でOnCompleteを呼び出す
        public async UniTask MoveAnimation()
        {
            // Startの位置を保存
            _startPosition = transform.position;

            Vector3 targetPosition = _targetObject.transform.position;

            float elapsedTime = 0.0f;

            while (elapsedTime < _duration)
            {
                elapsedTime += Time.deltaTime;
                // 0から1までの範囲に正規化
                float t = Mathf.Clamp01(elapsedTime / _duration);

                float xCurveValue = _animationCurveX.Evaluate(t);
                float yCurveValue = _animationCurveY.Evaluate(t);
                float zCurveValue = _animationCurveZ.Evaluate(t);

                // X軸Y軸の補完値を取得
                // (1 - t)*startPosition + t * endPosition (tは1になったら増えないようにする)
                float newX = Mathf.Lerp(_startPosition.x, targetPosition.x, xCurveValue);
                float newY = Mathf.Lerp(_startPosition.y, targetPosition.y, yCurveValue);
                float newZ = Mathf.Lerp(_startPosition.z, targetPosition.z, zCurveValue);

                transform.position = new Vector3(newX, newY, newZ);

                // 次のフレームまで待機
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        // 妖精が人形に入った演出の後に再生する
        public async UniTask FlashAnimation(GameObject particle)
        {
            // Particleの生成
            Vector3 spawnPosition = _targetObject.transform.position + new Vector3(0, 0, -1);
            GameObject flashParticle = Instantiate(particle, spawnPosition, Quaternion.identity);

            // particleの拡大用transform
            Transform particleTransform = flashParticle.transform;

            // particleの拡大アニメーション
            await particleTransform.DOScale(Vector3.one * 200f, 1f).SetEase(Ease.OutQuad);
            
            
        }
    }
}