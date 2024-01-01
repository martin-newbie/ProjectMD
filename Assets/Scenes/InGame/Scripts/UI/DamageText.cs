using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Text damageTxt;
    public Outline outLine;
    CanvasGroup alphaGroup;
    RectTransform rectTr;
    RectTransform canvasRect;
    DamageTextCanvas manager;

    public void InitText(DamageTextCanvas _manager)
    {
        alphaGroup = GetComponent<CanvasGroup>();
        rectTr = GetComponent<RectTransform>();

        manager = _manager;
        canvasRect = manager.canvasRect;
    }

    public void PrintDamage(float value, Vector3 target, int dir, ResistType resist, bool isCrit)
    {
        damageTxt.text = string.Format("{0:#,##}", value);
        alphaGroup.alpha = 1f;
        StartCoroutine(textMoving());

        IEnumerator textMoving()
        {
            var startPos = target.WorldToRectPosition(canvasRect);
            var endPos = startPos + new Vector3(Random.Range(50f, 100f) * dir, Random.Range(50f, 100f));

            switch (resist)
            {
                case ResistType.NORMAL:
                    damageTxt.fontSize = 25;
                    damageTxt.color = Color.white;

                    outLine.enabled = false;
                    break;
                case ResistType.WEAK:
                    damageTxt.fontSize = 15;
                    damageTxt.color = Color.gray;

                    outLine.enabled = true;
                    outLine.effectColor = Color.blue;
                    break;
                case ResistType.RESIST:
                    damageTxt.fontSize = 40;
                    damageTxt.color = new Color(1, 0.76f, 0);

                    outLine.enabled = true;
                    outLine.effectColor = new Color(0.77f, 0.25f, 0);
                    break;
            }

            float dur = 0.25f;
            float timer = 0f;

            while (timer < dur)
            {
                rectTr.anchoredPosition = Vector3.Lerp(startPos, endPos, 1 - Mathf.Pow(1 - (timer / dur), 5));
                timer += Time.deltaTime;
                yield return null;
            }

            timer = 0f;
            while (timer < dur)
            {
                alphaGroup.alpha = Mathf.Lerp(1, 0, 1 - Mathf.Pow(1 - (timer / dur), 5));
                timer += Time.deltaTime;
                yield return null;
            }

            manager.PushText(this);
            yield break;
        }
    }
}
