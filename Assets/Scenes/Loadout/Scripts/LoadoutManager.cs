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
        foreach (var item in infoButtons)
        {
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

        var mousePos = Input.mousePosition;
        var anchorPos = new Vector2();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(infoButtons[curDragIdx].GetComponent<RectTransform>(), mousePos, Camera.main, out anchorPos);

        infoButtons[curDragIdx].SetModelPos(anchorPos);
    }

    public void SwitchDragItem()
    {
        var prev = infoButtons[curDragIdx];
        var last = infoButtons.Find((item) => !item.isDown && item.isEnter);

        if (last == null)
        {
            prev.SetModelDefaultPos();
            prev.InitInfo(prev.LinkedData);
            curDragIdx = -1;
            return;
        }

        var prevData = prev.LinkedData;
        var lastData = last.LinkedData;

        prev.InitInfo(lastData);
        last.InitInfo(prevData);

        prev.SetModelDefaultPos();
        last.SetModelDefaultPos();

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

    public void OnGameStart()
    {
        if (deckIdxArr.Length <= 0) return;

        TempData.SetDeckUnit(deckIdxArr);
        SceneManager.LoadScene("InGame");
    }
}
