using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopMenu : MonoBehaviour
{
    DateTime lastEnergyTime;

    public Text energyTxt;
    public Text remainEnergyChargeTxt;

    void Start()
    {
        lastEnergyTime = UserData.Instance.GetEnergyTime();
        SetActiveIfEnergyAvaliable();
    }

    void Update()
    {
        var curTime = DateTime.UtcNow;

        if ((curTime.Millisecond - lastEnergyTime.Millisecond) / 6000 > 360)
        {
            UserData.Instance.UpdateEnergyTime(curTime);
            UserData.Instance.UpdateEnergy(1);
            SetActiveIfEnergyAvaliable();
        }

        energyTxt.text = $"{UserData.Instance.energy} / {UserData.Instance.GetMaxEnergy()}";

        if (!UserData.Instance.IsOverMaxEnergy())
        {
            remainEnergyChargeTxt.text = (lastEnergyTime + new TimeSpan(0, 6, 0) - curTime).ToString();
        }
    }

    void SetActiveIfEnergyAvaliable()
    {
        remainEnergyChargeTxt.gameObject.SetActive(!UserData.Instance.IsOverMaxEnergy());
    }
}
