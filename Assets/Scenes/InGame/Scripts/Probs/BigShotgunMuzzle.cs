using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShotgunMuzzle : MonoBehaviour
{
    [SerializeField] SpriteRenderer muzzleRenderer;
    [SerializeField] BoxCollider2D damageCollider;
    [SerializeField] ContactFilter2D filter;

    public void StartMuzzle(UnitBehaviour owner, DamageStruct damageInfo, UnitGroupType targetGroup)
    {

        List<Collider2D> overlapResults = new List<Collider2D>();
        Physics2D.OverlapCollider(damageCollider, filter, overlapResults);

        foreach (var item in overlapResults)
        {
            var unit = item.GetComponent<UnitObject>().behaviour;
            if (unit.group == targetGroup)
            {
                unit.OnDamage(damageInfo, owner);
            }
        }

        StartCoroutine(EffectRoutine(0.5f));
    }

    IEnumerator EffectRoutine(float dur)
    {
        float timer = 0f;
        muzzleRenderer.color = new Color(1, 1, 1, 1);
        while (timer < dur)
        {
            muzzleRenderer.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, timer / dur));
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
        yield break;
    }
}
