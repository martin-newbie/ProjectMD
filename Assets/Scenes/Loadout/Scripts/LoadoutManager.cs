using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public bool isInit = false;

    int[] deckIdxArr;
    int selectedDeck;
    int curDragIdx = -1;

    [SerializeField] List<LoadoutInfoUI> infoButtons;
    [SerializeField] List<Button> deckSelectButtons;
    [SerializeField] LoadoutSelectPanel selectPanel;
    DeckData[] decks;

    void Start()
    {
        for (int i = 0; i < infoButtons.Count; i++)
        {
            var item = infoButtons[i];
            item.InitButton(i);
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

        WebRequest.Post("test-loadout/deck-enter", UserData.Instance.uuid, (data) =>
        {
            var result = JsonUtility.FromJson<RecieveDeckData>(data);
            decks = result.decks;
            isInit = true;
            UpdateDeck(0);
        });
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

        if (last == null)
        {
            prev.SetModelDefaultPos();
        }
        else
        {
            last.skeleton.transform.localScale = Vector3.one * 1.25f;

            var temp = deckIdxArr[curDragIdx];
            deckIdxArr[prev.btnIdx] = deckIdxArr[last.btnIdx];
            deckIdxArr[last.btnIdx] = temp;
        }

        UpdateDeck(selectedDeck);
        curDragIdx = -1;
    }

    public void SelectDeck(int i)
    {
        selectedDeck = i;
        UpdateDeck(selectedDeck);
    }

    public void SetDeck(int[] indexes, int deckIdx)
    {
        var sendData = new SendDeckData();
        sendData.id = decks[deckIdx].id;
        sendData.user_uuid = decks[deckIdx].user_uuid;
        sendData.deck_index = deckIdx;
        sendData.unit1 = indexes[0];
        sendData.unit2 = indexes[1];
        sendData.unit3 = indexes[2];
        sendData.unit4 = indexes[3];
        sendData.unit5 = indexes[4];
        var sendJson = JsonUtility.ToJson(sendData);
        WebRequest.Post("test-loadout/deck-save", sendJson, (data) =>
        {
            decks[deckIdx].unit_indexes = indexes;
            UpdateDeck(deckIdx);
        });
    }

    public void UpdateDeck(int deckIdx)
    {
        deckIdxArr = decks[deckIdx].unit_indexes;
        for (int i = 0; i < infoButtons.Count; i++)
        {
            int id = deckIdxArr[i];
            if (id < 0)
            {
                infoButtons[i].InitInfo(null);
                continue;
            }

            var info = UserData.Instance.units.Find((item) => item.id == id);
            infoButtons[i].InitInfo(info);
            infoButtons[i].SetModelDefaultPos();
        }
    }

    public bool AlreadySelected(int unitIndex, int deckIndex)
    {
        bool result = false;
        for (int i = 0; i < decks.Length; i++)
        {
            if (deckIndex == i) continue;

            if (decks[i].unit_indexes.Contains(unitIndex))
            {
                result = true;
            }
        }

        return result;
    }

    public void OpenSelectPanel()
    {
        selectPanel.OpenPanel(deckIdxArr, selectedDeck);
    }

    public void OnGameStart()
    {
        if (deckIdxArr.Length <= 0) return;

        TempData.Instance.selectedDeck = selectedDeck;
        SceneManager.LoadScene("InGame");
    }

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}

[System.Serializable]
public class SendDeckData : DeckData
{
    public int unit1;
    public int unit2;
    public int unit3;
    public int unit4;
    public int unit5;
}

[System.Serializable]
public class RecieveDeckData
{
    public DeckData[] decks;
}