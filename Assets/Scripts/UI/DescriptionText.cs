using UnityEngine;

#if UNITY_EDITOR

/// <summary>オブジェクトに説明を付けたい時にアタッチするクラス</summary>
public class DescriptionText : MonoBehaviour
{
    [TextArea]
    public string _text;
}

#endif
