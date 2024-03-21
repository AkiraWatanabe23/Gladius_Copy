using System;
using UnityEngine;

[Serializable]
public abstract class PlayerSystemBase
{
    public virtual void Initialize(GameObject go) { }

    public virtual void OnUpdate() { }
}
