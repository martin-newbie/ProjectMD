using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallManager : MonoBehaviour
{
    [SerializeField] OverallStatusPanel statusPanel;

    [Header("Summary")]
    [SerializeField] Text nameText;
    [SerializeField] Text positionText;
    [SerializeField] Image positionImage;
    [SerializeField] Text atkTypeText;
    [SerializeField] Image atkTypeImage;
    [SerializeField] Text defTypeText;
    [SerializeField] Image defTypeImage;

    private void Start()
    {
        int unitId = TempData.Instance.selectedUnit;
        var unitData = UserData.Instance.FindUnitWithId(unitId);
        statusPanel.InitCharacter(unitData);
    }
}
