using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfoWindow : MonoBehaviour
{
    StageData linkedData;

    public void OpenStageInfo(StageData _linkedData)
    {
        gameObject.SetActive(true);
        linkedData = _linkedData;
    }

    public void OnStartButton()
    {
        var sendData = new SendUserData();
        sendData.uuid = UserData.Instance.uuid;
        WebRequest.Post("main-menu/enter-loadout", JsonUtility.ToJson(sendData), (data) =>
        {
            var recieveData = JsonUtility.FromJson<RecieveDeckData>(data);
            TempData.Instance.selectedChapter = linkedData.chapterIndex;
            TempData.Instance.selectedStage = linkedData.stageIndex;
            SceneLoadManager.Instance.LoadScene("Loadout", () => { LoadoutManager.Instance.InitLoadout(recieveData); });
        });
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
