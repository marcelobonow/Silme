using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryPopupManager : Dependable
{
    private static RetryPopupManager instance;


    public override bool HasInstance() => instance != null;
    public override void Instantiate()
    {
        Instantiate(gameObject);
        gameObject.SetActive(false);
    }
}
