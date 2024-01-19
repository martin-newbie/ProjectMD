using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBullet : ProjectileBehaviour
{
    public ProjectileBullet(Bullet _subject) : base(_subject)
    {
    }

    public override void ProjectileAction(Vector3 start, Vector3 end, float t)
    {
        float rotY = start.x < end.x ? 0 : 180;
        transform.rotation = Quaternion.Euler(0, rotY, 0);
        transform.position = Vector3.Lerp(start, end, t);
    }

}
