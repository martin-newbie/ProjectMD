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

        profileImage.sprite = ResourceManager.GetProfile(linkedData.charIdx);
    }

    public void OnInfoButtonClick()
    {

    }
}
