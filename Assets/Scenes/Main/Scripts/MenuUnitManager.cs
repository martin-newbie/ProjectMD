using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUnitManager : MonoBehaviour
{
    public float width;
    public MenuUnit[] unitObjs;

    private void Start()
    {
        foreach (var item in unitObjs)
        {
            item.InitButton(GetComponent<RectTransform>());
        }
    }

    public void InitUnits(int[] idxArr)
    {
        for (int i = 0; i < unitObjs.Length; i++)
        {
            var obj = unitObjs[i];
            if (i < idxArr.Length)
            {
                obj.gameObject.SetActive(true);
                obj.UpdateUnitIdx(idxArr[i]);
            }
            else
            {
                obj.gameObject.SetActive(false);
            }
        }

        SetUnitRandomPos(idxArr.Length);
    }

    void SetUnitRandomPos(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float randX = Random.Range(-width, width);
            var pos = new Vector2(randX, -130);
            unitObjs[i].GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
}
