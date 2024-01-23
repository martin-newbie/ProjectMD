using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrateEffect : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] SpriteRenderer sprite;


    Color defaultColor;
    Vector3 defaultScale;

    public void StartEffect()
    {
        defaultColor = sprite.color;
        defaultScale = transform.localScale;
        StartCoroutine(ActionRoutine());
    }

    IEnumerator ActionRoutine()
    {
        float timer = 0f;
        Color startColor = defaultColor;
        Vector3 startScale = defaultScale;
        Vector3 endScale = new Vector3(0.8f, 1.5f, 1f);

        while (timer < duration)
        {
            startColor.a = Mathf.Lerp(1, 0, timer / duration);
            sprite.color = startColor;
            transform.localScale = Vector3.Lerp(startScale, endScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
        yield break;
    }
}
