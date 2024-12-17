using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Title
{
    // 敵がオルゴールを盗む演出
    public class EnemySnatchAnimation : MonoBehaviour
    {
        [SerializeField, InspectorVariantName("何秒かけて移動するかの間隔")] protected float _duration;
        
        [SerializeField, InspectorVariantName("敵が止まる位置を設定")] private GameObject _targetPosition;

        [SerializeField] private Vector3 _rotation;

        private Vector3 _currentPosition;
        [SerializeField] private float _fadeDuration;

        // 初期位置を保存する
        public Vector3 GetCurrentPosition() => _currentPosition;


        private void Awake()
        {
            _currentPosition = transform.position;
        }

        // 小人が走って現れる
        public async UniTask Move()
        {
            await transform.DOMove(_targetPosition.transform.position, _duration).SetEase(Ease.InOutQuad);
        }

        public async UniTask Fade()
        {
            await transform.DOMove(_currentPosition, _fadeDuration).SetEase(Ease.InOutQuad).OnComplete(() => Destroy(gameObject));
        }

        // 人形を少し傾ける
        public async UniTask HeelOver()
        {
            await transform.DORotate(new Vector3(_rotation.x, _rotation.y, _rotation.z), 1);
        }
        
        protected void OnDestroy()
        {
            DOTween.Kill(transform);
        }
    }
}