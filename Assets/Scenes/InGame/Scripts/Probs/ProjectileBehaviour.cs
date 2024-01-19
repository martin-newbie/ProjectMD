using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehaviour
{
    public Bullet subject;
    public GameObject gameObject;
    public Transform transform;

    public ProjectileBehaviour(Bullet _subject)
    {
        subject = _subject;

        gameObject = subject.gameObject;
        transform = subject.transform;
    }

    public virtual void OnEnd(Vector3 endPos)
    {
        InGamePrefabsManager.PlayCommonHit(endPos);
    }

    public abstract void ProjectileAction(Vector3 start, Vector3 end, float t);

}
