using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUpgradePanel : MonoBehaviour
{

    UnitData linkedData;
    int selectIndex;
    int selectLevel;

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
        selectLevel = level;

        curSkillLevelText.text = $"Lv. {level + 1}";
        string nextLevelStr = level < 9 ? (level + 1).ToString() : "max";
        nextSkillLevelText.text = $"Lv. {nextLevelStr}";

        var requireItemInfo = ResourceManager.GetCommonSkillItemRequire(level);
        var unitItemInfo = StaticDataManager.GetSkillItemData(index);
        for (int i = 0; i < requireItems.Length; i++)
        {
            if (i < requireItemInfo.items.Length)
            {
                requireItems[i].InitItem(unitItemInfo.itemArray[requireItemInfo.items[i].idx], i, selectLevel);
            }
            else
            {
                requireItems[i].gameObject.SetActive(false);
            }
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
