using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public Transform[] backgrounds;
    public float minPos;

    public IEnumerator ScrollBackgroundFor(float dur, float speed)
    {
        float timer = 0f;
        while (timer < dur)
        {

            foreach (var item in backgrounds)
            {
                item.Translate(Vector3.left * speed * Time.deltaTime);

                if (item.position.x < -minPos)
                {
                    Vector3 pos = item.position;
                    pos.x += minPos * 2;
                    item.position = pos;
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
