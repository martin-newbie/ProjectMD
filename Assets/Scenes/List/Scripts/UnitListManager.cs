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

    private void Start()
    {
        InitList(UserData.Instance.units.ToArray());
    }

    public void InitList(UnitData[] units)
    {
        buttonList = new List<UnitListButton>();

        foreach (var unit in units)
        {
            var button = Instantiate(buttonPrefab, buttonContent);
            button.InitButton(unit);
            buttonList.Add(button);
        }
    }
}
