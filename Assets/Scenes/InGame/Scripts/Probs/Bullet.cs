using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ProjectileBehaviour projectile;

    public Coroutine StartBulletEffect(Vector3 start, Vector3 end, float moveSpeed, Action onFinish = null, int bulletType = 0)
    {
        SetBulletBehaviour(bulletType);
        return StartCoroutine(bulletMove(start, end, moveSpeed, onFinish));
    }

    protected virtual IEnumerator bulletMove(Vector3 start, Vector3 end, float moveSpeed, Action onFinish = null)
    {
        float dur = projectile.CalculateDuration(start, end, moveSpeed);
        float timer = 0f;

        while (timer < dur)
        {
            projectile.ProjectileAction(start, end, timer / dur);
            timer += Time.deltaTime;
            yield return null;
        }

        onFinish?.Invoke();
        projectile.OnEnd(end);
        yield break;
    }

    void SetBulletBehaviour(int idx)
    {
        switch (idx)
        {
            case 0:
                projectile = new ProjectileBullet(this);
                break;
            case 1:
                projectile = new ProjectileHowitzer(this);
                break;
            case 2:
                projectile = new ProjectileLaser(this);
                break;
        }
    }
}
