using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UserData
{
    public static UserData Instance = null;

    public int id;
    public string uuid;
    public string nickname;
    public int level;
    public float exp;
    public int dia;
    public int coin;
    public int energy;
    public string str_last_energy_updated;
    public List<UnitData> units;
    public List<StageResult> stage_result;
    public List<ItemData> items;

    public DateTime lastEnergyTime;

    public void InitUserData()
    {
        lastEnergyTime = DateTime.Parse(str_last_energy_updated).ToUniversalTime();
    }

    public int FindItemCount(int idx)
    {
        var item = FindItem(idx);
        if (item == null) return 0;
        else return item.count;
    }

    public ItemData FindItem(int idx)
    {
        return items.Find(item => item.idx == idx);
    }

    public ItemData[] FindSpeciesItems(int species)
    {
        return items.FindAll(item => StaticDataManager.GetItemValueData(item.idx).species == species).ToArray();
    }

    public UnitData FindUnitWithId(int id)
    {
        return units.Find(item => item.id == id);
    }

    public void UpdateExp(float extra)
    {
        exp += extra;
        // TODO : update level also
    }

    public void UpdateEnergy(int extra)
    {
        if (extra + energy < 0)
        {
            throw new Exception();
        }

        if (energy >= GetMaxEnergy() && energy + extra < GetMaxEnergy())
        {
            UpdateEnergyTime(DateTime.Now);
        }

        energy += extra;
    }

    public bool IsOverMaxEnergy()
    {
        return energy >= GetMaxEnergy();
    }

    public int GetMaxEnergy()
    {
        return 100 + level * 2;
    }

    public DateTime GetEnergyTime()
    {
        return lastEnergyTime;
    }

    public void UpdateEnergyTime(DateTime date)
    {
        lastEnergyTime = date;
    }

    public void AddItem(ItemData item)
    {
        if (items.Exists(e => e.idx == item.idx))
        {
            items.Find(e => e.idx == item.idx).count += item.count;
        }
        else
        {
            items.Add(item);
        }
    }

    public void UseManyItem(ItemData[] items)
    {
        foreach (var item in items)
        {
            UseItem(item);
        }
    }

    public void UseItem(ItemData item)
    {
        items.Find(e => e.idx == item.idx).count -= item.count;
    }
}

[Serializable]
public class ItemData
{
    public int idx;
    public int count;
}

[Serializable]
public class DeckData
{
    public int id;
    public int deck_index;
    public string title;
    public string user_uuid;

    public int[] unit_indexes;
}

[Serializable]
public class StageResult
{
    public int chapter_idx;
    public int stage_idx;
    public bool[] condition;
}