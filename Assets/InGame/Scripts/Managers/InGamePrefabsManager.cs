using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePrefabsManager : MonoBehaviour
{
    public static InGamePrefabsManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] List<GameObject> prefabs = new List<GameObject>();
    public Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();

    private void Start()
    {
        foreach (var item in prefabs)
        {
            prefabDic.Add(item.name, item);
        }
    }

    public static GameObject GetObject(string key)
    {
        return Instance.prefabDic[key];
    }
}
