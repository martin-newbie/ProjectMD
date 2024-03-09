using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfoWindow : MonoBehaviour
{
    public int chapter;
    public int stage;

    public void OpenStageInfo(int chapter, int stage)
    {
        this.chapter = chapter;
        this.stage = stage;
        gameObject.SetActive(true);
    }

    public void OnStartButton()
    {
        var sendData = new SendUserData();
        sendData.uuid = UserData.Instance.uuid;
        TempData.Instance.selectedChapter = chapter;
        TempData.Instance.selectedStage = stage;

        WebRequest.Post("main-menu/enter-loadout", JsonUtility.ToJson(sendData), (data) =>
        {
            var recieveData = JsonUtility.FromJson<RecieveDeckData>(data);
            SceneLoadManager.Instance.LoadScene("Loadout", () => { LoadoutManager.Instance.InitLoadout(recieveData); });
        });
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
