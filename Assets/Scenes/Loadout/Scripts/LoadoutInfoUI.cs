using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class LoadoutInfoUI : MonoBehaviour
{
    [Header("Upper")]
    [SerializeField] public SkeletonGraphic skeleton;
    [SerializeField] GameObject emptyBox;
    [SerializeField] GameObject infoBox;

    [Header("Profile")]
    [SerializeField] Text levelText;
    [SerializeField] Image positionImage;
    [SerializeField] Text nameText;
    [SerializeField] GameObject[] rankStar;

    [HideInInspector] public UnitData LinkedData;
    [HideInInspector] public int btnIdx;
    Vector2 defaultAnchorPos;
    public bool isDown;
    public bool isEnter;

    private void Start()
    {
        defaultAnchorPos = skeleton.rectTransform.anchoredPosition;
    }

    public void InitButton(int idx)
    {
        btnIdx = idx;
    }

    public void InitInfo(UnitData linkedData)
    {

        emptyBox.SetActive(linkedData == null);
        infoBox.SetActive(linkedData != null);
        skeleton.gameObject.SetActive(linkedData != null);

        if (linkedData == null)
        {
            return;
        }

        skeleton.UpdateSkeleton(ResourceManager.GetSkeleton(linkedData.index));

        if (LinkedData.index != linkedData.index)
        {
            SetAnimatoin("enter", true);
        }
        AddAnimation("wait", true);

        nameText.text = StaticDataManager.GetConstUnitData(linkedData.index).name;
        levelText.text = $"Lv.{LinkedData.level}";

        // check diffrence with previous
        // never change order
        LinkedData = linkedData;
        InitRankStar();
    }

    void InitRankStar()
    {
        int rank = LinkedData.rank;
        for (int i = 0; i < rankStar.Length; i++)
        {
            rankStar[i].SetActive(i < rank);
        }
    }

    public void SetModelPos(Vector2 anchorPos)
    {
        skeleton.rectTransform.anchoredPosition = anchorPos;
    }

    public void SetModelDefaultPos()
    {
        skeleton.rectTransform.anchoredPosition = defaultAnchorPos;
    }


    public void OnPointerDown()
    {
        isDown = true;
        LoadoutManager.Instance.SetDragTarget(this);
        SetAnimatoin("drag");
    }

    public void OnPointerEnter()
    {
        isEnter = true;
    }

    public void OnPointerExit()
    {
        isEnter = false;
    }

    public void OnPointerUp()
    {
        isDown = false;
        LoadoutManager.Instance.SwitchDragItem();
    }

    public void SetAnimatoin(string key, bool loop = false)
    {
        skeleton.AnimationState.SetAnimation(0, key, loop);
    }

    public void AddAnimation(string key, bool loop = false)
    {
        skeleton.AnimationState.AddAnimation(0, key, loop, 0);
    }

    public void OnInfoButtonClick()
    {

    }
}
