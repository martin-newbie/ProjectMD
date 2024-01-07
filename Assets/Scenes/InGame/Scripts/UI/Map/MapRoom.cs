using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoom : MonoBehaviour
{
    public GameObject[] corridors; // 0 : right, 1 : left, 2 : up, 3 : down
    public GameObject[] roomIcons; // 0 : none, 1 : start, 2 : battle, 3 : boss
    [HideInInspector] public RoomData LinkedData;
    [HideInInspector] public RectTransform rectTransform;

    public void InitRoom(RoomData linkedData)
    {
        LinkedData = linkedData;

        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(LinkedData.x * 125, LinkedData.y * 125);

        for (int i = 0; i < roomIcons.Length; i++)
        {
            roomIcons[i].SetActive((int)LinkedData.eventType == i);
        }

        foreach (var item in corridors)
        {
            item.SetActive(false);
        }
        for (int i = 0; i < LinkedData.connectedRooms.Length; i++)
        {
            int roomIdx = LinkedData.connectedRooms[i];
            var room = InGameMap.Instance.FindRoomByIndex(roomIdx);

            if(linkedData.x != room.x)
            {
                if (room.x > linkedData.x) corridors[0].SetActive(true);
                if (room.x < linkedData.x) corridors[1].SetActive(true);
            }
            if(linkedData.y != room.y)
            {
                if (room.y > linkedData.y) corridors[2].SetActive(true);
                if (room.y < linkedData.y) corridors[3].SetActive(true);
            }
        }
    }

    RoomData GetConnectedRoomData(int roomIdx)
    {
        return InGameMap.Instance.roomDataList.Find((item) => item.roomIdx == roomIdx);
    }

    public void OnButtonClick()
    {

    }
}
