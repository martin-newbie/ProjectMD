using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageListUnit : MonoBehaviour
{
    int index;
    StageListManager manager;

    public void InitUnit(StageListManager _manager, int idx)
    {
        manager = _manager;
        index = idx;
    }

    public void OnButtonClick()
    {
        // use index
        manager.ShowStageInfo(index);
    }
}
