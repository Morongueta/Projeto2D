using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    private SpriteRenderer render;
    public float blinkRate, duration;

    private float timer = 0f;

    public Color[] blinkColors = new Color[0];

    public bool start;

    private Coroutine Blinking;


    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();

        if(render == null)
        {
            render = GetComponentInChildren<SpriteRenderer>();
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(start)
        {
            Blinking = StartCoroutine(EBlinking());
        }
    }

    public void Remove()
    {
        if(Blinking != null)StopCoroutine(Blinking);
        render.color = Color.white;
        MonoBehaviour.Destroy(this);
    }

    private IEnumerator EBlinking()
    {
        if(render == null) yield break;

        start = false;
        int curColor = 0;
        int maxColor = blinkColors.Length + 1;
        Color col = Color.white;

        while(timer < duration)
        {
            if(curColor == 0)
            {
                col = Color.white;
            }else{
                col = blinkColors[curColor - 1];
            }
            
            render.color = col;
            yield return new WaitForSeconds(blinkRate);
            curColor++;
            if(curColor >= maxColor) curColor = 0;
        }
        render.color = Color.white;
        Destroy(this);
    }
}


public class Blink
{
    public static void Effect(GameObject target, float blinkRate = 0.015f, float duration = 0.15f, params Color[] blinkColors)
    {
        if(target.GetComponent<BlinkEffect>() != null) return;

        BlinkEffect effect = target.AddComponent<BlinkEffect>();

        effect.blinkRate = blinkRate;
        effect.duration = duration;
        effect.blinkColors = blinkColors;
        effect.start = true;
    }

    public static void Remove(GameObject target)
    {
        if(target.GetComponent<BlinkEffect>() == null) return;

        BlinkEffect effect = target.GetComponent<BlinkEffect>();

        if(effect != null)effect.Remove();
        
    }
}
