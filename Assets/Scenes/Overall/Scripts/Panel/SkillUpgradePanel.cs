using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUpgradePanel : MonoBehaviour
{

    UnitData linkedData;
    int selectIndex;

    [SerializeField] SkillInfoButton[] skillButtons;

    [Header("Info")]
    [SerializeField] Text skillTitleText;
    [SerializeField] Text curSkillLevelText;
    [SerializeField] Text curSkillDescText;
    [SerializeField] Text nextSkillLevelText;
    [SerializeField] Text nextSkillDescText;

    [Header("Item")]
    [SerializeField] SkillRequireItemInfo[] requireItems;
    [SerializeField] Text requireCoinText;

    public void OpenSkillUpgradePanel(UnitData linkedData, int selectIndex)
    {
        gameObject.SetActive(true);
        this.linkedData = linkedData;

        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].Init(i, linkedData.skill_level[i], linkedData.IsSkillLock(i), ChangeSkillInfoTo);
        }


        ChangeSkillInfoTo(selectIndex, linkedData.skill_level[selectIndex]);
    }

    void ChangeSkillInfoTo(int index, int level)
    {
        selectIndex = index;

        curSkillLevelText.text = $"Lv. {level + 1}";
        string nextLevelStr = level < 9 ? (level + 1).ToString() : "max";
        nextSkillLevelText.text = $"Lv. {nextLevelStr}";

        var requireItemInfo = DataManager.GetCommonSkillItem(level);
        var unitItemInfo = StaticDataManager.GetSkillItemData(index);
        for (int i = 0; i < requireItems.Length; i++)
        {
            if (i < requireItemInfo.items.Length)
            {
                requireItems[i].InitItem(unitItemInfo.itemArray[requireItemInfo.items[i].idx], i, linkedData.skill_level[selectIndex]);
            }
            else
            {
                requireItems[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnUpgradeButton()
    {
        if (linkedData.skill_level[selectIndex] >= 9) return;
        if (!OverallManager.Instance.CheckEnoughtItems(DataManager.GetCommonSkillItem(linkedData.skill_level[selectIndex]).items)) return;

        var sendData = new SendSkillLevelUp();
        sendData.uuid = UserData.Instance.uuid;
        sendData.id = linkedData.id;
        sendData.skill_index = selectIndex;
        sendData.use_items = DataManager.GetCommonSkillItem(linkedData.skill_level[selectIndex]).items;
        sendData.use_coin = 0; // TODO : it also should include in skillItemRequire
        WebRequest.Post("unit/upgrade-skill", JsonUtility.ToJson(sendData), (data) =>
        {
            linkedData.skill_level[sendData.skill_index]++;
            UserData.Instance.UseManyItem(sendData.use_items);
            UserData.Instance.coin -= sendData.use_coin;
            OpenSkillUpgradePanel(linkedData, selectIndex);
            OverallManager.Instance.statusPanel.OpenPanelObject(2);
        });
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class SendSkillLevelUp
{
    public string uuid;
    public int id;
    public int skill_index;
    public ItemData[] use_items;
    public int use_coin;
}