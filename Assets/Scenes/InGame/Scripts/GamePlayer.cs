using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer
{
    public GamePlayer()
    {

    }

    public List<UnitBehaviour> units = new List<UnitBehaviour>();

    // about skill
    public List<ActiveSkillBehaviour> skillUnits = new List<ActiveSkillBehaviour>();
    public float skillCost;
    public float skillCharging;

    public void SpawnUnits(int[] unitIdx, int[] posIdx)
    {
        // spawn units
    }

    public void Update()
    {
        skillCost += Time.deltaTime * skillCharging;
    }
}
