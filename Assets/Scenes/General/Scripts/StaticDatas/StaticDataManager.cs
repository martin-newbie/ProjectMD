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
        return Instance.unitStatus.dataList[keyIndex];
    }

    public StaticConstUnitData constUnitStatus;
    public static ConstUnitData GetConstUnitData(int keyIndex)
    {
        return Instance.constUnitStatus.dataList[keyIndex];
    }

    private void Start()
    {
        var staticDatas = new SheetDataBase[]
        {
            unitStatus,
            constUnitStatus,
        };

        foreach (var item in staticDatas)
        {
            item.LoadData();
        }
    }
}
