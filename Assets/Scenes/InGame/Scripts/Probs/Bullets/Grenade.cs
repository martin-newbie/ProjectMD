using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    float height = 1f;

    public Coroutine ShootGrenade(Vector3 start, Vector3 end, Action onComplete = null)
    {
        return StartCoroutine(GreneadeMove(start, end, onComplete));
    }

    IEnumerator GreneadeMove(Vector3 start, Vector3 end, Action onComplete = null)
    {
        float dur = 0.3f;
        float timer = 0f;

        while (timer < dur)
        {
            ProjectileAction(start, end, timer / dur);
            timer += Time.deltaTime;
            yield return null;
        }

        onComplete?.Invoke();
        Destroy(gameObject);
        yield break;
    }

    public void ProjectileAction(Vector3 start, Vector3 end, float t)
    {
        transform.position = GetPosition(start, end, t);

        var pos1 = GetPosition(start, end, t);
        var pos2 = GetPosition(start, end, t + Time.deltaTime);

        float eul = (pos2.y - pos1.y) / (pos2.x - pos1.x);
        transform.rotation = Quaternion.Euler(0, 0, eul * Mathf.Rad2Deg);
    }

    private Vector3 GetPosition(Vector3 start, Vector3 end, float t)
    {
        float targetX = start.x + (end.x - start.x) * t;
        float targetY = start.y + ((end.y - start.y)) * t + height * (1 - (Mathf.Abs(0.5f - t) / 0.5f) * (Mathf.Abs(0.5f - t) / 0.5f));
        return new Vector3(targetX, targetY);
    }
}
