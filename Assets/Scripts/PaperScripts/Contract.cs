using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contract : MonoBehaviour
{
    [SerializeField] private Vector2 negatePosition;
    [SerializeField] private Vector2 confirmPosition;

    [SerializeField] private float checkBoxSize;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireCube(negatePosition + (Vector2)transform.position, Vector2.one * checkBoxSize);
        Gizmos.DrawWireCube(confirmPosition + (Vector2)transform.position, Vector2.one * checkBoxSize);
    }
}
