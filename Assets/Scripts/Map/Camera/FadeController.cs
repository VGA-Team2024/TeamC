using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeController : MonoBehaviour
{
    [SerializeField]Image _img;
    /// <summary>
    /// フェードインの処理を行うメソッド
    /// </summary>
    /// <param name="duration">フェードインが完了するまでの時間</param>
    public void FadeIn(float duration)
    {
        _img.DOFade(0f,duration);
    }
    /// <summary>
    /// フェードアウトの処理を行うメソッド
    /// </summary>
    /// <param name="duration">フェードアウトが完了するまでの時間</param>
    public void FadeOut(float duration)
    {
        _img.DOFade(1f, duration);
    }
}
