using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldVerticalGroup : MonoBehaviour
{
    [SerializeField]private float spacing;
    [SerializeField]private Vector2 startPoint;
    [SerializeField]private Vector2 offsetPoint;
    private void Start()
    {
        Vertical();
    }


    public void Vertical()
    {
        if(transform.childCount == 0) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = ((startPoint + offsetPoint * (GetComponent<RectTransform>().sizeDelta / 2f)) * (GetComponent<RectTransform>().sizeDelta / 2f)) - (Vector2.up * spacing) * i;
        }
    }
}
