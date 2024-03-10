using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance = null;

    public static string chapterKey = "chapter";

    public List<ChapterData> chapters = new List<ChapterData>();

    private void Awake()
    {
        Instance = this;
        Load();
    }

    void Load()
    {
        chapters = new List<ChapterData>();
        int chapterCount = 1;
        for (int i = 0; i < chapterCount; i++)
        {
            var chapterTextAsset = Resources.Load<TextAsset>($"Stages/{i}");
            var chapter = JsonUtility.FromJson<ChapterData>(chapterTextAsset.text);
            chapters.Add(chapter);
        }
    }
}
