using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHowitzer : ProjectileBehaviour
{
    float height;

    public ProjectileHowitzer(Bullet _subject) : base(_subject)
    {
        height = 1f;
    }

    public override void ProjectileAction(Vector3 start, Vector3 end, float t)
    {
        float targetX = start.x + (end.x - start.x) * t;
        float targety = start.y + ((end.y - start.y)) * t + height * (1 - (Mathf.Abs(0.5f - t) / 0.5f) * (Mathf.Abs(0.5f - t) / 0.5f));
        transform.position = new Vector3(targetX, targety);
    }
}
