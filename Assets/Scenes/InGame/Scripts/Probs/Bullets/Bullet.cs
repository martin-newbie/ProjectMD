using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Coroutine ShootBullet(Vector3 start, Vector3 end, float moveSpeed, Action onFinish = null)
    {
        return StartCoroutine(bulletMove(start, end, moveSpeed, onFinish));
    }

    protected IEnumerator bulletMove(Vector3 start, Vector3 end, float moveSpeed, Action onFinish = null)
    {
        float dur = Vector3.Distance(start, end) / moveSpeed;
        float timer = 0f;

        float rotY = start.x < end.x ? 0 : 180;
        transform.rotation = Quaternion.Euler(0, rotY, 0);

        while (timer < dur)
        {
            transform.position = Vector3.Lerp(start, end, timer / dur);
            timer += Time.deltaTime;
            yield return null;
        }

        onFinish?.Invoke();
        yield break;
    }
}
