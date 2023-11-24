using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] ContactFilter2D filter;
    [SerializeField] Animation anim;
    [SerializeField] Collider2D thisCol;

    public void StartExplosion(UnitGroupType targetGroup, float damage, UnitBehaviour from)
    {
        gameObject.SetActive(true);
        anim.Play();

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

        StartCoroutine(ExplosionCoroutine());
    }

    IEnumerator ExplosionCoroutine()
    {
        yield return new WaitForSeconds(anim.GetClip("fromBottomExplosion").length);
        Destroy(gameObject);
        yield break;
    }
}
