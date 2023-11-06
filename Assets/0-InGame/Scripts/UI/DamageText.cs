using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Text damageTxt;
    RectTransform rectTr;
    DamageTextCanvas manager;

    public void InitText(DamageTextCanvas _manager)
    {
        rectTr = GetComponent<RectTransform>();
        manager = _manager;
    }

    public void PrintDamage(float value, Transform target, ResistType resist, bool isCrit)
    {
        damageTxt.text = string.Format("{0:#,##}", value);
        StartCoroutine(textMoving());

        IEnumerator textMoving()
        {
            var startPos = target.ToInterfacePosition(rectTr);
            var endPos = new Vector3(Random.Range(10f, 25f), Random.Range(10f, 25f));

            float dur = 0.5f;
            float timer = 0f;

            while (timer < dur)
            {
                rectTr.anchoredPosition = Vector3.Lerp(startPos, endPos, 1 - Mathf.Pow(1 - (timer / dur), 5));
                timer += Time.deltaTime;
                yield return null;
            }

            manager.PushText(this);
            yield break;
        }
    }
}
