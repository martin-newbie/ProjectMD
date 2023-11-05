using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextCanvas : MonoBehaviour
{
    public static DamageTextCanvas Instance;
    private void Awake()
    {
        Instance = this;
    }


}
