using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitListManager : MonoBehaviour
{
    public static UnitListManager Instance = null;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] UnitListButton buttonPrefab;
    [SerializeField] Transform buttonContent;
    List<UnitListButton> buttonList;

    void Start()
    {
        buttonList = new List<UnitListButton>();
        var unitDatas = UserData.Instance.units;
        foreach (var unit in unitDatas)
        {
            var button = Instantiate(buttonPrefab, buttonContent);
            button.InitButton(unit);
            buttonList.Add(button);
        }
    }
}
