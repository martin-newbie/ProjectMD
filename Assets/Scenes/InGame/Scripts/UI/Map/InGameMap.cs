using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InGameMap : MonoBehaviour
{
    public static InGameMap Instance = null;
    private void Awake()
    {
        Instance = this;
    }

    [HideInInspector] public List<RoomData> roomDataList;
    [HideInInspector] public List<CombatData> combatDataList;

    public RectTransform userIcon;
    public MapRoom roomPrefab;
    public Transform roomParent;
    List<MapRoom> allRooms;

    public int curRoomIdx = 0;

    public void InitMap(MapData linkedData)
    {
        roomDataList = new List<RoomData>(linkedData.roomDatas);

        foreach (var item in roomDataList)
        {
            var room = Instantiate(roomPrefab, roomParent);
            room.InitRoom(item);
            allRooms.Add(room);
        }
    }

    public void MoveToMap(RoomData moveRoom)
    {
        var curRoom = FindRoomByIndex(curRoomIdx);

        if(curRoom == moveRoom)
        {
            return;
        }
        if (!curRoom.connectedRooms.ToList().Contains(moveRoom.roomIdx))
        {
            return;
        }

        // move effect

    }

    public RoomData FindRoomByIndex(int idx)
    {
        return roomDataList.Find((item) => item.roomIdx == idx);
    }

    public MapRoom FindRoomObjectByIndex(int idx)
    {
        return allRooms.Find((item) => item.LinkedData.roomIdx == idx);
    }
}
