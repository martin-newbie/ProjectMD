using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDataManager : MonoBehaviour
{
    public static StaticDataManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public StaticUnitStatus unitStatus;
    public static UnitStatus GetUnitStatus(int keyIndex)
    {
        return Instance.unitStatus.dataList.Find(item => item.keyIndex == keyIndex);
    }

    public StaticConstUnitData constUnitStatus;
    public static ConstUnitData GetConstUnitData(int keyIndex)
    {
        return Instance.constUnitStatus.dataList.Find(item => item.keyIndex == keyIndex);
    }

    public StaticUnitSkillStatus unitSkillStatus;
    public static UnitSkillStatus GetUnitSkillStatus(int keyIndex)
    {
        return Instance.unitSkillStatus.dataList.Find(item => item.unitIndex == keyIndex);
    }

    public StaticItemValueData itemValueData;
    public static ItemValueData GetItemValueData(int index)
    {
        return Instance.itemValueData.dataList[index];
    }

    public StaticSkillRequireItemData skillRequireData;
    public static SkillRequireItemData GetSkillItemData(int index)
    {
        return Instance.skillRequireData.dataList.Find(item => item.index == index);
    }

    public StaticEquipmentValueData equipmentData;
    public static EquipmentValueData GetEquipmentValueData(int index)
    {
        return Instance.equipmentData.dataList[index];
    }

    private void Start()
    {
        var staticDatas = new SheetDataBase[]
        {
            unitStatus,
            constUnitStatus,
            unitSkillStatus,
            itemValueData,
            skillRequireData,
            equipmentData,
        };

        foreach (var item in staticDatas)
        {
            item.LoadData();
        }
    }
}
