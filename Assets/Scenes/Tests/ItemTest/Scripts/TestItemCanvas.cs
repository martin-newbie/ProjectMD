using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemCanvas : MonoBehaviour
{
    public void GetItem(int idx)
    {
        var item = new ItemData();
        item.idx = idx;
        item.count = 1;
        WebRequest.Post("user/get-item", JsonUtility.ToJson(item), (data) =>
        {
            var recieve = JsonUtility.FromJson<RecieveGetItem>(data);
            UserData.Instance.AddItem(item);
        });
    }
}

public class RecieveGetItem
{

}
