using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ゲーム
/// </summary>
public class GameEventRecorder
{
    static GameEventRecorder _instance = new GameEventRecorder();
    private GameEventRecorder() { }

    class EventRecord
    {
        public string GameHash;
        public DateTime GameStartTime;
        public bool IsReview;
    };

    EventRecord _currentGame = null;

    static public void GameStart()
    {
        if (_instance._currentGame != null)
        {
            Debug.LogWarning("既にプレイ中のゲームがあるようです");
        }

        //ゲームハッシユと現時点の時間を記録する
        _instance._currentGame = new EventRecord();
        _instance._currentGame.GameHash = Guid.NewGuid().ToString();
        _instance._currentGame.GameStartTime = DateTime.Now;
    }

    static public void GameReview(Action reviewEndCallback)
    {
        _instance._currentGame.IsReview = true;
    }

    static public void GameEnd(Action reviewEndCallback)
    {
        if (_instance._currentGame.IsReview == false)
        {
            GameReview(reviewEndCallback);
        }
        _instance._currentGame = null;
    }
}