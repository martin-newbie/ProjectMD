using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleEffect : MonoBehaviour
{
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
