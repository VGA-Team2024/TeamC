using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Title
{
    public class Fade : MonoBehaviour
    {
        [SerializeField] private Image _fadeImage;
        [SerializeField] private Material _fadeMaterial;

        private static readonly int FadeThreshold = Shader.PropertyToID("_FadeThreshold");

        private void Start()
        {
            _fadeImage.material = new Material(_fadeMaterial);
        }

        // 画面を暗くする
        public void Fadeout(float duration)
        {
            // 初期化
            _fadeImage.material.SetFloat(FadeThreshold, 1f);

            DOTween.To(
                () => _fadeImage.material.GetFloat(FadeThreshold),
                x => _fadeImage.material.SetFloat(FadeThreshold, x),
                0f,
                // イージングでフェードの仕方を調整する
                duration).SetEase(Ease.Linear);
        }
    }
}