using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class LoadoutInfoUI : MonoBehaviour
{
    [Header("Upper")]
    [SerializeField] SkeletonGraphic skeleton;
    [SerializeField] GameObject emptyBox;
    [SerializeField] GameObject infoBox;

    [Header("Profile")]
    [SerializeField] Image profileImage;
    [SerializeField] Text levelText;
    [SerializeField] Text tearText;

    [Header("Name")]
    [SerializeField] Image positionImage;
    [SerializeField] Text nameText;

    [Header("Attribute")]
    [SerializeField] Text atkAttText;
    [SerializeField] Text defAttText;

    CharacterData LinkedData;

    public void InitInfo(CharacterData linkedData)
    {
        emptyBox.SetActive(linkedData == null);
        infoBox.SetActive(linkedData != null);
        skeleton.gameObject.SetActive(linkedData != null);

        if (linkedData == null)
        {
            return;
        }

        skeleton.Update(0f);
        skeleton.skeletonDataAsset = ResourceManager.GetSkeleton(linkedData.charIdx);
        skeleton.Initialize(true);

        if (LinkedData != linkedData)
        {
            skeleton.AnimationState.SetAnimation(0, "enter", true);
        }
        skeleton.AnimationState.AddAnimation(0, "wait", true, 0);

        profileImage.sprite = ResourceManager.GetProfile(linkedData.charIdx);

        LinkedData = linkedData;
    }

    public void OnInfoButtonClick()
    {

    }
}
