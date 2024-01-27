using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    void Start()
    {
        WebRequest.Post("game-menu/menu-enter", "", (data) =>
        {

        });

    }

    public void OnButtonGameStage()
    {
        WebRequest.Post("game-menu/into-stage", "", (data) =>
        {
            TempData.Instance.selectedGameMode = GameMode.STAGE;
            SceneManager.LoadScene("Loadout");
        });
    }

    public void OnButtonStory()
    {
        WebRequest.Post("game-menu/into-story", "", (data) =>
        {

        });
    }

    public void OnButtonRaid()
    {
        WebRequest.Post("game-menu/into-raid", "", (data) =>
        {
            TempData.Instance.selectedGameMode = GameMode.RAID;
            SceneManager.LoadScene("Loadout");
        });
    }

    public void OnButtonPVP()
    {
        WebRequest.Post("game-menu/into-pvp", "", (data) =>
        {
            TempData.Instance.selectedGameMode = GameMode.PVP;
            SceneManager.LoadScene("Loadout");
        });
    }

    public void OnButtonDungeon()
    {
        WebRequest.Post("game-menu/into-dungeon", "", (data) =>
        {
            TempData.Instance.selectedGameMode = GameMode.DUNGEON;
            SceneManager.LoadScene("Loadout");
        });
    }

    public void OnButtonTest()
    {
        TempData.Instance.selectedGameMode = GameMode.TEST;
        SceneManager.LoadScene("Loadout");
    }
}
