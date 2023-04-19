using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gatto;

public class MenuController : MonoBehaviour
{
    public void PlayButton()
    {
        TransitionManager.i.PlayScene(TransitionStyle.FADE, TransitionState.IN, TransitionState.OUT, "Game");
    }

    public void CreditsButton()
    {
        
    }

    public void QuitButton()
    {
        TransitionManager.i.Play(TransitionStyle.FADE, TransitionState.IN, ()=>{
            Application.Quit();
        });
        
    }
}
