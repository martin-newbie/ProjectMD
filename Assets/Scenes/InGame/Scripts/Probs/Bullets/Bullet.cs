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

        var dir = end - start;
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);

        while (timer < dur)
        {
            transform.position = Vector3.Lerp(start, end, timer / dur);
            timer += Time.deltaTime;
            yield return null;
        }

        onFinish?.Invoke();
        Destroy(gameObject);
        yield break;
    }
}
