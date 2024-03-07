using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopMenu : MonoBehaviour
{
    DateTime lastEnergyTime;

    void Start()
    {
        lastEnergyTime = UserData.Instance.GetEnergyTime();
    }

    void Update()
    {
        var curTime = DateTime.Now;

        if ((curTime.Millisecond - lastEnergyTime.Millisecond) / 6000 > 360)
        {
            UserData.Instance.UpdateEnergyTime(curTime);
            UserData.Instance.UpdateEnergy(1);
        }
    }
}
