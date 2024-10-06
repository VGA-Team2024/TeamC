using System;
using UnityEngine;

/// <summary>抽象クラス</summary>
/// <typeparam name="T">MonoBehaviourを継承している型のみ指定</typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T I
    {
        get
        {
            if (_instance == null)
            {
                Type t = typeof(T);

                //シーン内のT型のオブジェクトを検索してセット
                _instance = (T)FindAnyObjectByType(t);
                if (_instance == null)
                {
                    Debug.LogError(t + "をアタッチしているオブジェクトはありません");
                }
            }

            return _instance;
        }
    }

    //インスタンスが存在してるかをチェック
    //インスタンスよりも先に参照してしまう場合はこっちでチェックする
    public static bool ExistInstance()
    {
        return _instance != null;
    }

    protected void Awake()
    {
        CheckInstance();
    }

    //このクラスがすでにシングルトンインスタンスとして設定しているかをチェックする関数
    protected bool CheckInstance()
    {
        if (_instance == null)
        {
            _instance = this as T;
            return true;
        }
        else if (I == this)
        {
            return true;
        }

        Destroy(this);
        return false;
    }
}