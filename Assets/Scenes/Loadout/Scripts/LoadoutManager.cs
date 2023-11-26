using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager Instance = null;
    private void Awake()
    {
        Instance = this;
    }

    int[] deckIdxArr;
    [SerializeField] LoadoutInfoUI[] infoButtons;
    [SerializeField] LoadoutSelectPanel selectPanel;

    private void Start()
    {
        TestUserData();
        foreach (var item in infoButtons)
        {
            item.InitInfo(null);
        }
        selectPanel.InitPanel();
        deckIdxArr = new int[0];
    }

    void TestUserData()
    {
        UserData user = new UserData();
        for (int i = 0; i < 20; i++)
        {
            user.charDatas.Add(new CharacterData(i));
        }
    }


    public void UpdateDeck(int[] idxArr)
    {
        deckIdxArr = idxArr;
        for (int i = 0; i < infoButtons.Length; i++)
        {
            if(i < deckIdxArr.Length)
            {
                int charIdx = idxArr[i];
                var info = UserData.Instance.charDatas.Find((item) => item.charIdx == charIdx);
                infoButtons[i].InitInfo(info);
            }
            else
            {
                infoButtons[i].InitInfo(null);
            }
        }
    }

    public void OpenSelectPanel()
    {
        selectPanel.OpenPanel(deckIdxArr);
    }
}
