using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutTitleEdit : MonoBehaviour
{
    int deckIdx;
    DeckData linkedData;

    [SerializeField] InputField titleInput;

    public void OpenTitleEdit(int _deckIdx, DeckData _linkedData)
    {
        gameObject.SetActive(true);

        deckIdx = _deckIdx;
        linkedData = _linkedData;

        titleInput.text = linkedData.title;
    }

    public void OnConfirm()
    {
        linkedData.title = titleInput.text;

        var sendData = new SendTitleChange();
        sendData.id = linkedData.id;
        sendData.title = linkedData.title;
        var sendJson = JsonUtility.ToJson(sendData);

        WebRequest.Post("loadout/change-title", sendJson, (data) =>
        {
            LoadoutManager.Instance.UpdateDeck(deckIdx);
            gameObject.SetActive(false);
        });
    }
}

[System.Serializable]
class SendTitleChange
{
    public int id;
    public string title;
}