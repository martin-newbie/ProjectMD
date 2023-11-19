using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameModeBehaviour
{
    public abstract IEnumerator GameModeRoutine();
}
