using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public Action inFrontEvent;

    private void Start()
    {
        SetupEvent();
    }

    public virtual void SetupEvent()
    {
        inFrontEvent = null;
    }
}
