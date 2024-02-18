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
        transform.position = GetPositionT(start, end, t);

        var pos1 = GetPositionT(start, end, t);
        var pos2 = GetPositionT(start, end, t + Time.deltaTime);

        float eul = (pos2.y - pos1.y) / (pos2.x - pos1.x);
        transform.rotation = Quaternion.Euler(0, 0, eul * Mathf.Rad2Deg);
    }

    private Vector3 GetPositionT(Vector3 start, Vector3 end, float t)
    {
        float targetX = start.x + (end.x - start.x) * t;
        float targetY = start.y + ((end.y - start.y)) * t + height * (1 - (Mathf.Abs(0.5f - t) / 0.5f) * (Mathf.Abs(0.5f - t) / 0.5f));
        return new Vector3(targetX, targetY);
    }
}
