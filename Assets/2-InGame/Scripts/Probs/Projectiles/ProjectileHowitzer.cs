using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHowitzer : ProjectileBehaviour
{
    public ProjectileHowitzer(Bullet _subject) : base(_subject)
    {
    }

    public override void ProjectileAction(Vector3 start, Vector3 end, float t)
    {
        throw new System.NotImplementedException();
    }
}
