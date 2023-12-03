using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] ContactFilter2D filter;
    [SerializeField] Collider2D thisCol;

    public void StartExplosion(UnitGroupType targetGroup, float damage, UnitBehaviour from)
    {
        gameObject.SetActive(true);

        List<Collider2D> overlapResults = new List<Collider2D>();
        Physics2D.OverlapCollider(thisCol, filter, overlapResults);
        if(overlapResults.Count > 0)
        {
            foreach (var item in overlapResults)
            {
                var unit = item.GetComponent<UnitObject>().behaviour;
                if(unit.group == targetGroup)
                {
                    unit.OnDamage(damage, from);
                }
            }
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
