using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePrefabsManager : MonoBehaviour
{
    public static InGamePrefabsManager Instance;
    private void Awake()
    {
        Instance = this;
        foreach (var item in prefabs)
        {
            prefabDic.Add(item.name, item);
        }
    }

    [SerializeField] List<GameObject> prefabs = new List<GameObject>();
    public Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();

    public static GameObject GetObject(string key)
    {
        return Instance.prefabDic[key];
    }

    public static void PlayCommonHit(Vector3 pos)
    {
        string key = "hit_effect";
        var obj = Instantiate(GetObject(key));
        var anim = obj.GetComponent<Animator>();

        obj.transform.position = pos;
        anim.SetTrigger("trigger");
        Destroy(obj, 0.18f);
    }
}
