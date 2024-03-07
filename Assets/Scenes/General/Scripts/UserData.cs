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

    public DateTime lastEnergyTime;

    public void InitUserData()
    {
        lastEnergyTime = DateTime.Parse(str_last_energy_updated).ToUniversalTime();
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