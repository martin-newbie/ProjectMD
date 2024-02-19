using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicardySkillSyringe : MonoBehaviour
{
    public float speed;
    public Transform model;
    public ParticleSystem debris;

    public void ObjectMove(Vector3 start, Vector3 end, Action endAction = null)
    {
        model.transform.rotation = Quaternion.Euler(0, 0, start.x < end.x ? 0 : 180);
        StartCoroutine(moveRoutine());

        IEnumerator moveRoutine()
        {
            debris.Play();

            float dur = Vector3.Distance(start, end) / speed;
            float timer = 0f;

            while (timer < dur)
            {
                transform.position = Vector3.Lerp(start, end, timer / dur);
                timer += Time.deltaTime;
                yield return null;
            }

            endAction?.Invoke();
            debris.Stop();

            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
            yield break;
        }
    }

}
