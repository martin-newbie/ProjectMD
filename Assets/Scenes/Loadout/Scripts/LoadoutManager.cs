using System;
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

    int[] deckIdxArr;
    int selectedDeck;
    int curDragIdx = -1;

    [SerializeField] Button deckButtonPrefab;
    [SerializeField] Transform deckButtonsParent;
    List<Button> deckButtons;

    [SerializeField] List<LoadoutInfoUI> infoButtons;
    [SerializeField] LoadoutSelectPanel selectPanel;
    [HideInInspector] public List<DeckData> decks;

    [SerializeField] Text deckTitleTxt;
    [SerializeField] LoadoutTitleEdit titleEdit;

    public void InitLoadout(RecieveDeckData recieveData)
    {
        for (int i = 0; i < infoButtons.Count; i++)
        {
            var item = infoButtons[i];
            item.InitButton(i);
            item.InitInfo(null);
        }

        selectPanel.InitPanel();
        deckIdxArr = new int[0];

        decks = recieveData.decks.ToList();

        deckButtons = new List<Button>();
        for (int i = 0; i < decks.Count; i++)
        {
            AddDeckButton(i);
        }

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

    public void OnAddDeck()
    {
        WebRequest.Post("loadout/deck-add", UserData.Instance.uuid, (data) =>
        {
            var recieve = JsonUtility.FromJson<RecieveDeckAdd>(data);
            var deck = recieve.deck;
            decks.Add(deck);
            AddDeckButton(deck.deck_index);
        });
    }

    private Button AddDeckButton(int idx)
    {
        var button = Instantiate(deckButtonPrefab, deckButtonsParent);
        button.transform.SetSiblingIndex(idx);
        button.GetComponentInChildren<Text>().text = decks[idx].title;
        button.onClick.AddListener(() => SelectDeck(idx));
        deckButtons.Add(button);
        return button;
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
        WebRequest.Post("loadout/deck-save", sendJson, (data) =>
        {
            decks[deckIdx].unit_indexes = indexes;
            UpdateDeck(deckIdx);
        });
    }

    public void UpdateDeck(int deckIdx)
    {
        deckIdxArr = decks[deckIdx].unit_indexes;
        deckTitleTxt.text = decks[deckIdx].title;
        deckButtons[deckIdx].GetComponentInChildren<Text>().text = decks[deckIdx].title;

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
        for (int i = 0; i < decks.Count; i++)
        {
            if (deckIndex == i) continue;

            if (decks[i].unit_indexes.Contains(unitIndex))
            {
                result = true;
            }
        }

        return result;
    }

    public void OnTitleEditEnter()
    {
        titleEdit.OpenTitleEdit(selectedDeck, decks[selectedDeck]);
    }

    public void OpenSelectPanel()
    {
        selectPanel.OpenPanel(deckIdxArr, selectedDeck);
    }

    public void OnGameStart()
    {
        if (deckIdxArr.Length <= 0) return; // print error log

        var sendDeck = new SendAllDeck();
        sendDeck.decks = new DeckData[decks.Count];
        for (int i = 0; i < decks.Count; i++)
        {
            sendDeck.decks[i] = decks[i];
        }
        var sendData = JsonUtility.ToJson(sendDeck);

        WebRequest.Post("loadout/deck-save-all", sendData, (data) =>
        {
            TempData.Instance.selectedDeck = selectedDeck;
        });

        var sendGameEnter = new SendGameEnter();
        sendGameEnter.uuid = UserData.Instance.uuid;
        sendGameEnter.energy_use = 10;
        sendGameEnter.deck_index = TempData.Instance.selectedDeck;
        sendGameEnter.selected_chapter = TempData.Instance.selectedChapter;
        sendGameEnter.selected_stage = TempData.Instance.selectedStage;

        WebRequest.Post("ingame/game-enter", JsonUtility.ToJson(sendGameEnter), (data) =>
        {
            var recieveData = JsonUtility.FromJson<RecieveGameEnter>(data);
            SceneLoadManager.Instance.LoadScene("InGame", () =>
            {
                InGameManager.Instance.InitGameMode(recieveData);
            }, false);
        });
    }
}

[Serializable]
public class SendDeckData : DeckData
{
    public int unit1;
    public int unit2;
    public int unit3;
    public int unit4;
    public int unit5;
}

[Serializable]
public class SendAllDeck
{
    public DeckData[] decks;
}

[Serializable]
public class RecieveDeckData
{
    public DeckData[] decks;
}

[Serializable]
public class RecieveDeckAdd
{
    public DeckData deck;
}

[Serializable]
public class SendGameEnter
{
    public string uuid;
    public int energy_use;
    public int deck_index;
    public int selected_stage;
    public int selected_chapter;
}

[Serializable]
public class RecieveGameEnter
{
    public bool success;
    public DeckData deck;
    public StageData stage_data;
}