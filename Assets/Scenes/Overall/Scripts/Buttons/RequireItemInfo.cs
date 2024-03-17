using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequireItemInfo : MonoBehaviour
{
    [SerializeField] ItemInfo itemInfo;
    [SerializeField] Text countText;

    public void InitItem(int itemIndex, int slotIndex, int skillLevel, ItemData requireItem)
    {
        gameObject.SetActive(true);
        itemInfo.InitInfo(itemIndex);

        var item = UserData.Instance.FindItem(itemIndex);
        int itemCount = item != null ? item.count : 0;
        int requireItemCount = requireItem.count;
        countText.text = $"{itemCount} / {requireItemCount}";
    }
}
