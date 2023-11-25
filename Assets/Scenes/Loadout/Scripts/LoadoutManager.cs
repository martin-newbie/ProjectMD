using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutManager : MonoBehaviour
{
    int[] deckIdxArr;
    [SerializeField] CharacterInfoUI[] infoButtons;

    private void Start()
    {
        TestUserData();
        foreach (var item in infoButtons)
        {
            item.InitInfo(null);
        }
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
                infoButtons[i].InitInfo(UserData.Instance.charDatas[idxArr[i]]);
            }
            else
            {
                infoButtons[i].InitInfo(null);
            }
        }
    }
}
