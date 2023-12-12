using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUnitManager : MonoBehaviour
{
    public float width;
    public SkeletonGraphic[] unitObjs;

    public void InitUnits(int[] idxArr)
    {
        for (int i = 0; i < unitObjs.Length; i++)
        {
            var obj = unitObjs[i];
            if (i < idxArr.Length - 1)
            {
                obj.gameObject.SetActive(true);
                obj.Update(0f);
                obj.skeletonDataAsset = ResourceManager.GetSkeleton(idxArr[i]);
                obj.Initialize(true);
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
            unitObjs[i].rectTransform.anchoredPosition = pos;
        }
    }
}
