using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // ����Ʈ�νḸ ���ǰ� ���� ������ ������ �������� ����

    public float moveSpeed;

    public Coroutine StartBulletEffect(Vector3 start, Vector3 end, Action onFinish = null)
    {
        return StartCoroutine(bulletMove());

        IEnumerator bulletMove()
        {
            float rotY = start.x < end.x ? 0 : 180;
            transform.rotation = Quaternion.Euler(0, rotY, 0);

            float dur = Vector3.Distance(start, end) / moveSpeed;
            float timer = 0f;

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
}
