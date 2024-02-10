using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour
{
    public void OnMissionButton()
    {

    }

    public void OnUnitListButton()
    {

    }

    public void OnPresetButton()
    {

    }

    public void OnItemButton()
    {

    }

    public void OnGachaButton()
    {

    }

    public void OnMailButton()
    {

    }

    public void OnGameButton()
    {
        SceneLoadManager.Instance.LoadScene("GameMenu");
    }
}
