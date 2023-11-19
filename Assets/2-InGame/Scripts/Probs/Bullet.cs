using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ProjectileBehaviour behaviour;

    public Coroutine StartBulletEffect(Vector3 start, Vector3 end, float moveSpeed, Action onFinish = null, int bulletType = 0)
    {
        SetBulletBehaviour(bulletType);
        return StartCoroutine(bulletMove());

        IEnumerator bulletMove()
        {
            float dur = Vector3.Distance(start, end) / moveSpeed;
            float timer = 0f;

            while (timer < dur)
            {
                behaviour.ProjectileAction(start, end, timer / dur);
                timer += Time.deltaTime;
                yield return null;
            }

            onFinish?.Invoke();
            Destroy(gameObject);
            yield break;
        }
    }

    void SetBulletBehaviour(int idx)
    {
        switch (idx)
        {
            case 0:
                behaviour = new ProjectileBullet(this);
                break;
        }
    }
}
