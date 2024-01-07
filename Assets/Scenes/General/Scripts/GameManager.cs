using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        new TempData();
        new UserData();
    }

}
