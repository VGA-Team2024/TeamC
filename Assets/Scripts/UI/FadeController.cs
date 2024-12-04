using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeController : MonoBehaviour
{
    [SerializeField]Image _img;
    /// <summary>
    /// �t�F�[�h�C���̏������s�����\�b�h
    /// </summary>
    /// <param name="duration">�t�F�[�h�C������������܂ł̎���</param>
    public void FadeIn(float duration)
    {
        _img.DOFade(0f,duration);
    }
    /// <summary>
    /// �t�F�[�h�A�E�g�̏������s�����\�b�h
    /// </summary>
    /// <param name="duration">�t�F�[�h�A�E�g����������܂ł̎���</param>
    public void FadeOut(float duration)
    {
        _img.DOFade(1f, duration);
    }
}
