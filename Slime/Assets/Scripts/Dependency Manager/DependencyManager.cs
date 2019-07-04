using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependencyManager : MonoBehaviour
{
    [SerializeField] private List<Dependable> dependencies;

    private void Awake()
    {
        foreach(var dependency in dependencies)
        {
            if(!dependency.HasInstance())
                dependency.Instantiate();
        }
    }
}
