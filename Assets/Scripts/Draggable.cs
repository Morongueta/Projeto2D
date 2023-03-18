using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public void GoForward()
    {
        SpriteRenderer rend = GetComponentInChildren<SpriteRenderer>();
        TextMeshPro[] texts = GetComponentsInChildren<TextMeshPro>();

        rend.sortingOrder = 10;

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].sortingOrder = 10;
        }
    }

    public void GoBackward()
    {
        SpriteRenderer[] rends = FindObjectsOfType<SpriteRenderer>();
        TextMeshPro[] texts = FindObjectsOfType<TextMeshPro>();

        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].sortingOrder--;
        }

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].sortingOrder--;
        }


    }

}
