using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameModeBehaviour
{
    public IEnumerator GameModeRoutine();
    public void Update();
}
