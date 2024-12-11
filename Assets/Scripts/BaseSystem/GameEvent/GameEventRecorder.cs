using System;
using UnityEditor;
using UnityEngine;
using GameEvent;

/// <summary>
/// ゲームのイベントを記録する
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

    /// <summary>
    /// ゲーム開始時に呼び出す
    /// </summary>
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
        if (_instance._currentGame == null)
        {
            Debug.LogWarning("プレイ中のゲームがないようです。暫定で現時点をGameStartとします");

            GameStart();
        }

        _instance._currentGame.IsReview = true;
        ReviewWindow.Build(reviewEndCallback);
    }

    static public void GameEnd(Action reviewEndCallback)
    {
        if (_instance._currentGame == null)
        {
            Debug.LogWarning("プレイ中のゲームがないようです。暫定で現時点をGameStartとします");

            GameStart();
        }

        if (_instance._currentGame.IsReview == false)
        {
            GameReview(reviewEndCallback);
        }
        _instance._currentGame = null;
    }

    static public void SendEventData(GameEventData data)
    {

    }
}