using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInput : MonoBehaviour
{

    private static bool onArcade;

    public static CustomInput i;
    private void Awake()
    {
        if(i == null)
        {
            i = this;
            DontDestroyOnLoad(this);
            return;
        }
        Destroy(this.gameObject);
    }
    private void Start()
    {
        CustomMouse.i.SetSimulatedMouse(onArcade);
    }
    private void Update()
    {
        if(Input.GetButtonDown("VERDE1"))
        {
            onArcade = !onArcade;

            CustomMouse.i.SetSimulatedMouse(onArcade);
        }
    }

    public static bool GetKeyDown(KeyCode key, string arcadeButton)
    {
        if(onArcade)
        {
            return Input.GetButtonDown(arcadeButton);
        }else{
            return Input.GetKeyDown(key);
        }
    }

    public static bool GetKey(KeyCode key, string arcadeButton)
    {
        if(onArcade)
        {
            return Input.GetButton(arcadeButton);
        }else{
            return Input.GetKey(key);
        }
    }
    public static bool GetKeyUp(KeyCode key, string arcadeButton)
    {
        if(onArcade)
        {
            return Input.GetButtonUp(arcadeButton);
        }else{
            return Input.GetKeyUp(key);
        }
    }
}
