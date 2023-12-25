using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager Instance = null;
    private void Awake()
    {
        Instance = this;
    }

    int[] deckIdxArr;
    int showIdx;
    int curDragIdx = -1;

    [SerializeField] List<LoadoutInfoUI> infoButtons;
    [SerializeField] List<Button> deckSelectButtons;
    [SerializeField] LoadoutSelectPanel selectPanel;

    IEnumerator Start()
    {
        for (int i = 0; i < infoButtons.Count; i++)
        {
            var item = infoButtons[i];
            item.InitButton(i++);
            item.InitInfo(null);
        }

        for (int i = 0; i < deckSelectButtons.Count; i++)
        {
            var item = deckSelectButtons[i];
            int idx = i;
            item.onClick.AddListener(() => SelectDeck(idx));
        }
        selectPanel.InitPanel();
        deckIdxArr = new int[0];

        yield return null;
        UpdateDeck(0);
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

        for (int i = 0; i < infoButtons.Count; i++)
        {
            if (curDragIdx != i && infoButtons[i].isEnter)
            {
                infoButtons[i].skeleton.transform.localScale = Vector3.one;
            }
            else
            {
                infoButtons[i].skeleton.transform.localScale = Vector3.one * 1.25f;
            }
        }
    }

    public void SwitchDragItem()
    {
        var prev = infoButtons[curDragIdx];
        var last = infoButtons.Find((item) => item != prev && item.isEnter);
        last.skeleton.transform.localScale = Vector3.one * 1.25f;

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

        UpdateDeck(showIdx);
        curDragIdx = -1;
    }

    public void SelectDeck(int i)
    {
        showIdx = i;
        UpdateDeck(showIdx);
    }

    public void UpdateDeck(int deckIdx)
    {
        deckIdxArr = UserData.Instance.allDeckUnits[deckIdx];
        for (int i = 0; i < infoButtons.Count; i++)
        {
            if (i < deckIdxArr.Length)
            {
                int charIdx = deckIdxArr[i];
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
        selectPanel.OpenPanel(deckIdxArr, showIdx);
    }

    public void OnGameStart()
    {
        if (deckIdxArr.Length <= 0) return;
        SceneManager.LoadScene("InGame");
    }
}
