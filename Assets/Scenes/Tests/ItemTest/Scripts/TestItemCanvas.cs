using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemCanvas : MonoBehaviour
{
    public void GetItem(int idx)
    {
        var item = new SendGetItem();
        item.uuid = UserData.Instance.uuid;
        item.item_idx = idx;
        item.count = 1;
        WebRequest.Post("user/get-item", JsonUtility.ToJson(item), (data) =>
        {
            var recieve = JsonUtility.FromJson<RecieveGetItem>(data);
            var getItem = new ItemData();
            getItem.idx = item.item_idx;
            getItem.count = item.count;
            UserData.Instance.AddItem(getItem);
        });
    }
}

[System.Serializable]
public class SendGetItem
{
    public string uuid;
    public int item_idx;
    public int count;
}

[System.Serializable]
public class RecieveGetItem
{

}
