using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ColorUtil : MonoBehaviour
{
    public Action<Color> OnUpdate;
    public Action<Color> OnFinish;

    public Action<ColorBlock> OnUpdateBlock;
    public Action<ColorBlock> OnFinishBlock;

    public Color target;
    public Color current;

    public ColorBlock targetBlock;
    public ColorBlock currentBlock;

    public bool startBlock, startColor;

    public float timer;

    private float countTimer;

    private void Update()
    {
        if(startColor)UpdateColor();
        if(startBlock)UpdateBlock(); 
    }

    void UpdateColor()
    {
        if(timer == 0f) current = target;
        current = Color.Lerp(current, target, timer * Time.deltaTime);

        OnUpdate?.Invoke(current);
        current.a = Mathf.Lerp(current.a, target.a, timer * Time.deltaTime);

        if(current == target) OnFinish?.Invoke(current);
    }

    void UpdateBlock()
    {
        if(timer == 0f)
        {
            currentBlock = targetBlock;
            
            OnUpdateBlock?.Invoke(currentBlock);

            OnFinishBlock?.Invoke(currentBlock);
            return;
        }
        currentBlock.normalColor = Color.Lerp(currentBlock.normalColor, targetBlock.normalColor, timer * Time.deltaTime);
        currentBlock.pressedColor = Color.Lerp(currentBlock.pressedColor, targetBlock.pressedColor, timer * Time.deltaTime);
        currentBlock.selectedColor = Color.Lerp(currentBlock.selectedColor, targetBlock.selectedColor, timer * Time.deltaTime);
        currentBlock.highlightedColor = Color.Lerp(currentBlock.highlightedColor, targetBlock.highlightedColor, timer * Time.deltaTime);
        currentBlock.disabledColor = Color.Lerp(currentBlock.disabledColor, targetBlock.disabledColor, timer * Time.deltaTime);

        OnUpdateBlock?.Invoke(currentBlock);

        if(currentBlock.normalColor == targetBlock.normalColor) OnFinishBlock?.Invoke(currentBlock);
    }


    public void StartColor(Color start, Color target, float timer, Action<Color> OnUpdate, Action<Color> OnFinish)
    {
        this.timer = timer;
        this.target = target;
        this.current = start;

        this.OnUpdate += OnUpdate;
        this.OnFinish += OnFinish;

        startColor = true;
    }

    public void StartColorBlock(ColorBlock start, ColorBlock target, float timer, Action<ColorBlock> OnUpdate, Action<ColorBlock> OnFinish)
    {
        this.timer = timer;
        this.targetBlock = target;
        this.currentBlock = start;

        this.OnUpdateBlock += OnUpdate;
        this.OnFinishBlock += OnFinish;

        startBlock = true;
    }
}

