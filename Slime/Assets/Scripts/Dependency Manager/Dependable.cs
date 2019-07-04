using UnityEngine;
using System;

public abstract class Dependable : MonoBehaviour
{
    public abstract bool HasInstance();
    public abstract void Instantiate();
}
