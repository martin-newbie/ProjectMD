using System.Collections.Generic;
using UnityEngine;

public enum MapEventType
{
    NONE,
    START,
    BATTLE,
    BOSS,
}

[System.Serializable]
public class MapData
{
    public List<RoomData> roomDatas;
    public List<CombatData> combatDatas;

    public MapData(RoomData[] room, CombatData[] combat)
    {
        roomDatas = new List<RoomData>(room);
        combatDatas = new List<CombatData>(combat);
    }
}

[System.Serializable]
public class RoomData
{
    public int x, y;
    public int roomIdx;
    public MapEventType eventType;
    public int eventIdx;
    public int[] connectedRooms;

    public RoomData(int _x, int _y, int _room, int _eventType, int _eventIdx)
    {
        x = _x;
        y = _y;
        roomIdx = _room;
        eventType = (MapEventType)_eventType;
        eventIdx = _eventIdx;
    }
}

[System.Serializable]
public class CombatData
{
    public int eventIdx;

    public int waveCount;
    public Vector3[][] poses;
    public int[][] units;

    public CombatData(Vector3[][] _poses, int[][] _units)
    {
        poses = _poses;
        units = _units;
        waveCount = units.Length;
    }
}