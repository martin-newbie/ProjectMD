using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextCanvas : MonoBehaviour
{
    public static DamageTextCanvas Instance;
    private void Awake()
    {
        Instance = this;
    }

    public RectTransform canvasRect;
    public DamageText damagePrefab;
    Stack<DamageText> textPool = new Stack<DamageText>();

    private void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            var temp = Instantiate(damagePrefab, transform);
            temp.InitText(this);
            temp.gameObject.SetActive(false);
            textPool.Push(temp);
        }
    }

    public void PrintDamageText(float value, Vector3 target, int dir, ResistType resist, bool isCrit = false)
    {
        var temp = textPool.Pop();
        temp.gameObject.SetActive(true);
        temp.PrintDamage(value, target, dir, resist, isCrit);
    }

    public void PushText(DamageText text)
    {
        text.gameObject.SetActive(false);
        textPool.Push(text);
    }
}
