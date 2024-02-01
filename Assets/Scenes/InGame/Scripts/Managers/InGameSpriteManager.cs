using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSpriteManager : MonoBehaviour
{
    public static InGameSpriteManager Instance = null;
    private void Awake()
    {
        Instance = this;
    }

    public Sprite asisGrenadeSprite;
    public Sprite lazerSprite;
}
