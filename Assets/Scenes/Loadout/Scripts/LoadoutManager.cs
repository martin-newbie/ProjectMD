using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager Instance = null;
    private void Awake()
    {
        Instance = this;
    }

    int[] deckIdxArr;
    int curDragIdx = -1;

    [SerializeField] List<LoadoutInfoUI> infoButtons;
    [SerializeField] LoadoutSelectPanel selectPanel;

    private void Start()
    {
        int i = 0;
        foreach (var item in infoButtons)
        {
            item.InitButton(i++);
            item.InitInfo(null);
        }
        selectPanel.InitPanel();
        deckIdxArr = new int[0];
    }


    private void Update()
    {
        SetDragPos();
    }

    public void SetDragTarget(LoadoutInfoUI button)
    {
        if (button.LinkedData == null) return;

        int idx = infoButtons.IndexOf(button);
        curDragIdx = idx;
    }

    void SetDragPos()
    {
        if (curDragIdx < 0) return;

        var anchorPos = Input.mousePosition.MouseToRectPosition(infoButtons[curDragIdx].GetComponent<RectTransform>());
        infoButtons[curDragIdx].SetModelPos(anchorPos);
    }

    public void SwitchDragItem()
    {
        var prev = infoButtons[curDragIdx];
        var last = infoButtons.Find((item) => !item.isDown && item.isEnter);

        if (last == null)
        {
            prev.SetModelDefaultPos();
        }
        else
        {
            var temp = deckIdxArr[curDragIdx];
            deckIdxArr[curDragIdx] = deckIdxArr[last.btnIdx];
            deckIdxArr[last.btnIdx] = temp;
        }

        UpdateDeck(deckIdxArr);
        curDragIdx = -1;
    }

    public void UpdateDeck(int[] idxArr)
    {
        deckIdxArr = idxArr;
        for (int i = 0; i < infoButtons.Count; i++)
        {
            if (i < deckIdxArr.Length)
            {
                int charIdx = idxArr[i];
                var info = UserData.Instance.unitDatas.Find((item) => item.unitIdx == charIdx);
                infoButtons[i].InitInfo(info);
            }
            else
            {
                infoButtons[i].InitInfo(null);
            }

            infoButtons[i].SetModelDefaultPos();
        }
    }

    public void OpenSelectPanel()
    {
        selectPanel.OpenPanel(deckIdxArr);
    }

    public void OnGameStart()
    {
        if (deckIdxArr.Length <= 0) return;

        TempData.SetDeckUnit(deckIdxArr);
        SceneManager.LoadScene("InGame");
    }
}
