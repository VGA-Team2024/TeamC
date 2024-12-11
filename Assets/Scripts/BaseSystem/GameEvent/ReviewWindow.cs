using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using GameEvent;
using UnityEngine.AddressableAssets;

/// <summary>
/// ゲームのイベントを記録する
/// </summary>
public class ReviewWindow : MonoBehaviour
{
    [SerializeField] Button _sendButton;
    Action _sendCallback;

    static public ReviewWindow Build(Action callback)
    {
        var obj = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Review.prefab").WaitForCompletion();
        Utility.Instantiate(obj);
        Addressables.Release(obj);
        ReviewWindow review = obj.GetComponent<ReviewWindow>();
        review._sendCallback = callback;

        return review;
    }
}