using System;
using UnityEngine;
using GameEvent;
using Cysharp.Threading.Tasks;

/// <summary>
/// ゲームのイベントを記録する
/// </summary>
public class GameEventRecorder
{
    const bool UnityEditorTest = false;

    static GameEventRecorder _instance = new GameEventRecorder();
    private GameEventRecorder() { }

    class EventRecord
    {
        public string GameHash;
        public DateTime GameStartTime;
        public bool IsReview;
        public bool IsGameEnd;
    };

    [Serializable]
    public class CommonEventData
    {
        [SerializeField] string EventName;
        [SerializeField] string TeamID;
        [SerializeField] string BuildHash;
        [SerializeField] string GameHash;
        [SerializeField] int GamePlayTime;
        [SerializeField] GameEventData Payload;

        private CommonEventData() { }
        public CommonEventData(string evtName, GameEventData payload)
        {
            EventName = evtName;
            TeamID = BuildState.TeamID;
            BuildHash = BuildState.BuildHash;
            if (_instance._currentGame != null)
            {
                GameHash = _instance._currentGame.GameHash;
                GamePlayTime = DateTime.Compare(_instance._currentGame.GameStartTime, DateTime.Now);
            }
            Payload = payload;
        }
    };


    const string baseuri = "https://jyl5w9zfz3.execute-api.ap-northeast-1.amazonaws.com/release/";
    EventRecord _currentGame = null;

    static bool IsSkipAction => (UnityEditorTest == false && BuildState.BuildHash == "UNITY_EDITOR")

    /// <summary>
    /// ゲーム開始時に呼び出す
    /// </summary>
    static public void GameStart()
    {
        if (IsSkipAction) return;

        if (_instance._currentGame != null)
        {
            Debug.LogWarning("既にプレイ中のゲームがあるようです");
        }

        //ゲームハッシユと現時点の時間を記録する
        _instance._currentGame = new EventRecord();
        _instance._currentGame.GameHash = Guid.NewGuid().ToString();
        _instance._currentGame.GameStartTime = DateTime.Now;

        _instance.SendStart();
    }

    static public void GameReview(Action reviewEndCallback)
    {
        if (IsSkipAction) return;

        if (_instance._currentGame == null)
        {
            Debug.LogWarning("プレイ中のゲームがないようです。暫定で現時点をGameStartとします");

            GameStart();
        }

        _instance._currentGame.IsReview = true;
        ReviewWindow.Build(_instance.SendReview, reviewEndCallback);
    }

    static public void GameEnd(Action reviewEndCallback)
    {
        if (IsSkipAction) return;

        if (_instance._currentGame == null)
        {
            Debug.LogWarning("プレイ中のゲームがないようです。暫定で現時点をGameStartとします");

            GameStart();
        }
        
        _instance._currentGame.IsGameEnd = true;

        if (_instance._currentGame.IsReview == false)
        {
            GameReview(reviewEndCallback);
        }
        else
        {
            _instance._currentGame = null;
        }
    }

    async void SendStart()
    {
        if (IsSkipAction) return;

        var data = new GameEventData();
        data.DataPack("Time", _instance._currentGame.GameStartTime);
        var packet = new CommonEventData("GameStart", data);
        string json = JsonUtility.ToJson(packet);
        var res = await Network.WebRequest.PostRequest(baseuri + "Event", packet);
        Debug.Log(res);
    }

    async void SendReview(int star, string comment)
    {
        if (IsSkipAction) return;

        var data = new GameEventData();
        data.DataPack("StarNum", star);
        data.DataPack("Comment", comment);
        var packet = new CommonEventData("Review", data);
        await Network.WebRequest.PostRequest(baseuri + "GameReview", packet);

        if (_instance._currentGame != null && _instance._currentGame.IsGameEnd)
        {
            _instance._currentGame = null;
        }
    }

    static public async void SendEventData(string situation, GameEventData data)
    {
        if (IsSkipAction) return;

        var packet = new CommonEventData(situation, data);
        await Network.WebRequest.PostRequest(baseuri + "Event", packet);
    }
}