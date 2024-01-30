using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLaser : ProjectileBehaviour
{
    SpriteRenderer sprite;

    public ProjectileLaser(Bullet _subject) : base(_subject)
    {
    }

    public override float CalculateDuration(Vector3 start, Vector3 end, float moveSpeed)
    {
        sprite = subject.GetComponent<SpriteRenderer>();
        sprite.size = new Vector2(0.4f, Vector3.Distance(start, end));
        int dir = start.x > end.x ? -1 : 1;
        sprite.transform.rotation = Quaternion.Euler(0, 0, dir * 90);
        return 0.5f;
    }

    public override void ProjectileAction(Vector3 start, Vector3 end, float t)
    {
        sprite.color = new Color(1, 1, 1, 1 - t);
    }
}
