using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// includes in ingame manager
public class StageManager : MonoBehaviour
{
    string path = "Stages/";
    List<ChapterData> chapterStagesData;

    void Start()
    {
        chapterStagesData = new List<ChapterData>();
        var textAssets = Resources.LoadAll<TextAsset>(path);
        foreach (var item in textAssets)
        {
            var chapterData = JsonUtility.FromJson<ChapterData>(item.text);
            chapterStagesData.Add(chapterData);
        }
    }

    public List<StageUnitData> GetStageEnemies(int chapterIdx, int stageIdx)
    {
        List<StageUnitData> unitDatas = new List<StageUnitData>();
        var stageData = chapterStagesData[chapterIdx].stageDatas[stageIdx];
        foreach (var wave in stageData.waveDatas)
        {
            foreach (var unit in wave.unitDatas)
            {
                if (!unitDatas.Exists(target => target.index == unit.index && target.unit_type == unit.unit_type))
                {
                    unitDatas.Add(unit);
                }
            }
        }

        return unitDatas;
    }
}

[Serializable]
public class WaveData
{
    public List<StageUnitData> unitDatas;

    public WaveData()
    {
        unitDatas = new List<StageUnitData>();
    }
}

[Serializable]
public class StageData
{
    public int chapterIndex;
    public int stageIndex;
    public int stageLevel;

    public List<WaveData> waveDatas;

    public StageData()
    {
        waveDatas = new List<WaveData>();
    }
}

[Serializable]
public class ChapterData
{
    public int chapterIndex;
    public List<StageData> stageDatas;

    public ChapterData()
    {
        stageDatas = new List<StageData>(new StageData[20]);
    }
}

[Serializable]
public class StageUnitData : UnitData
{
    public int unit_type; // 0: native, 1: elite, 2: boss
}