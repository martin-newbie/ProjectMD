using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageListUnit : MonoBehaviour
{
    int index;
    StageListManager manager;

    public void InitUnit(StageListManager _manager, int idx)
    {
        index = idx;
    }

    public void OnButtonClick()
    {
        // use index
        manager.ShowStageInfo(index);
    }
}
