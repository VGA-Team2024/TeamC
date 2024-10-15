using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>　ポータルの組み合わせを保存する構造体 </summary>
[Serializable]
public struct Potal
{
    [SerializeField] private Collider _portal_A;
    [SerializeField] private Collider _portal_B;

    public Collider Portal_A => _portal_A;
    public Collider Portal_B => _portal_B;
}

/// <summary> マップ毎の情報を保存する構造体 </summary>
[Serializable]
public struct Map
{
    [SerializeField, Header("マップのプレハブ")] private GameObject _exitMapPrefab;
    [SerializeField, Header("マップの入り口となるポータルのリスト")] private List<Collider> _entrancePortals;    // ここにあるトリガーに入ったらexitMapをアクティブにする

    /// <summary> 出口があるマップ </summary>
    public GameObject ExitMapPrefab => _exitMapPrefab;
    
    /// <summary> 入り口となるポータルのリスト </summary>
    public List<Collider> EntranceColliders => _entrancePortals;
}

/// <summary> マップの情報を保存するクラス </summary>
public class MapManager : MonoBehaviour
{
    [SerializeField, Header("マップとそこにつながる出口のリスト")] private List<Map> _mapData;
    [SerializeField, Header("ポータルの組み合わせのリスト")] private List<Potal> _spawnerPositionPair;

    public List<Potal> SpawnerPositionPair => _spawnerPositionPair;
    public List<Map> MapData => _mapData;
}