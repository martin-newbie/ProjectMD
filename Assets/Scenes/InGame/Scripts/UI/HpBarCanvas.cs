using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarCanvas : MonoBehaviour
{
    public static HpBarCanvas Instance = null;
    private void Awake()
    {
        Instance = this;
    }

    public HpBarBase commonHpBar;

    public HpBarBase GetHpBar(int type)
    {
        HpBarBase hpBar = null;
        
        switch (type)
        {
            case 0:
                hpBar = Instantiate(commonHpBar, transform) as HpBarBase;
                break;
            default:
                break;
        }

        return hpBar;
    }
}
