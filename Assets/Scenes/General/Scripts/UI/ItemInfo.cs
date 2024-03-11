using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text count;

    public void InitInfo(int index)
    {
        var item = UserData.Instance.items.Find(item => item.idx == index);

        if (item == null)
        {
            count.text = "0";
        }
        else
        {
            count.text = item.count.ToString();
        }
    }
}
