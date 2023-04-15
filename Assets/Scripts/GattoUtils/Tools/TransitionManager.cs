using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TransitionStyle //Types of Transition
{
    FADE,
    PASS,
    CIRCLE
}
public enum TransitionState //States of Transition
{
    IN,
    OUT
}

public class TransitionManager : MonoBehaviour
{
    [SerializeField]private bool playOnStart;
    [SerializeField]private TransitionStyle which;
    [SerializeField]private Transition[] transitions; //Stores information of different types of transition
    public static TransitionManager i; //Give to all scripts an access to play transitions

    private bool TransitionPlaying; //An Check if transition is playing

    

    private void Awake()
    {
        //Make the object "TransitionManager" Unique
        if(i != null)
        {
            Destroy(this.gameObject);
            return;
        }
        i = this;
        DontDestroyOnLoad(this.gameObject.transform.parent.gameObject);
    }

    private void Start()
    {
        if(playOnStart)
        {
            Play(which, TransitionState.OUT, null);
        }
    }

    public void Play(TransitionStyle style, TransitionState start, Action OnEnd = null) //Simple Code to Play a Transition
    {
        StartCoroutine(EPlayTransition(style,start, OnEnd));
    }

    //Simple Code to Play a Transition with an end
    public void Play(TransitionStyle style, TransitionState start, TransitionState end, Action OnMiddle = null, Action OnEnd = null)
    {
        StartCoroutine(EPlayTransition(style,start,end,OnMiddle, OnEnd));
    }

    public void PlayScene(TransitionStyle style, TransitionState start, TransitionState end, string targetScene, Action CustomSceneChange = null, Action OnEnd = null)
    {
        StartCoroutine(EPlaySceneTransition(style,start,end,targetScene,CustomSceneChange, OnEnd));
    }

    public void PlayScene(TransitionStyle style, TransitionState start, string targetScene, Action CustomSceneChange = null, Action OnEnd = null)
    {
        StartCoroutine(EPlaySceneTransition(style,start,targetScene,CustomSceneChange, OnEnd));
    }

    //Simple Code to Play a Transition with an end
    public void Play(TransitionStyle style, TransitionState end, Action OnStart = null, Action OnEnd = null)
    {
        StartCoroutine(EPlayTransition(style, end, OnStart, OnEnd));
    }

    //The Code that runs the Transition
    private IEnumerator EPlayTransition(TransitionStyle style, TransitionState start, Action OnEnd = null)
    {
        //Setup Transition
        Transition transition = GetTransition(style);
        GameObject transitionObj = Instantiate(transition.transitionObject, transform);
        Animator transitionAnim = transitionObj.GetComponent<Animator>();

        string startAnim = start.ToString().ToLower();

        //Start Transition
        TransitionPlaying = true;

        //Play Start Transition
        transitionAnim.Play(startAnim);

        //Wait Transition Finish
        yield return new WaitForSeconds(1f);

        //Wait To End
        yield return new WaitForSeconds(.15f);
        TransitionPlaying = false;
        Destroy(transitionObj);
        OnEnd?.Invoke();
    }


    private IEnumerator EPlayTransition(TransitionStyle style, TransitionState start, TransitionState end, Action OnMiddle = null, Action OnEnd = null)
    {
        //Setup Transition
        Transition transition = GetTransition(style);
        GameObject transitionObj = Instantiate(transition.transitionObject, transform);
        Animator transitionAnim = transitionObj.GetComponent<Animator>();

        string startAnim = start.ToString().ToLower();
        string endAnim = end.ToString().ToLower();

        //Start Transition
        TransitionPlaying = true;

        //Play Start Transition
        transitionAnim.Play(startAnim);

        //Wait Transition Finish
        yield return new WaitForSeconds(1f);
        OnMiddle?.Invoke();

        //Play End Transition
        transitionAnim.Play(endAnim);

        //Wait Transition Finish
        yield return new WaitForSeconds(1f);
        //End Transition

        //Wait To End
        yield return new WaitForSeconds(.15f);
        TransitionPlaying = false;

        Destroy(transitionObj);
        OnEnd?.Invoke();
    }
    private IEnumerator EPlaySceneTransition(TransitionStyle style, TransitionState start, TransitionState end, string sceneTarget, Action CustomSceneChange = null, Action OnEnd = null)
    {
        //Setup Transition
        Transition transition = GetTransition(style);
        GameObject transitionObj = Instantiate(transition.transitionObject, transform);
        Animator transitionAnim = transitionObj.GetComponent<Animator>();

        string startAnim = start.ToString().ToLower();
        string endAnim = end.ToString().ToLower();

        //Start Transition
        TransitionPlaying = true;

        //Play Start Transition
        transitionAnim.Play(startAnim);

        //Wait Transition Finish
        yield return new WaitForSeconds(1f);

        if(CustomSceneChange == null)
        {
            SceneManager.LoadScene(sceneTarget);
        }else{
            CustomSceneChange?.Invoke();
        }


        //Wait until transition to scene finish
        while(SceneManager.GetActiveScene().name != sceneTarget)
        {
            yield return null;
        }

        //Play End Transition
        transitionAnim.Play(endAnim);

        //Wait Transition Finish
        yield return new WaitForSeconds(1f);
        //End Transition

        //Wait To End
        yield return new WaitForSeconds(.15f);
        TransitionPlaying = false;

        Destroy(transitionObj);
        OnEnd?.Invoke();
    }

    private IEnumerator EPlaySceneTransition(TransitionStyle style, TransitionState start, string sceneTarget, Action CustomSceneChange = null, Action OnEnd = null)
    {
        //Setup Transition
        Transition transition = GetTransition(style);
        GameObject transitionObj = Instantiate(transition.transitionObject, transform);
        Animator transitionAnim = transitionObj.GetComponent<Animator>();

        string startAnim = start.ToString().ToLower();

        //Start Transition
        TransitionPlaying = true;

        //Play Start Transition
        transitionAnim.Play(startAnim);

        if(CustomSceneChange == null)
        {
            SceneManager.LoadScene(sceneTarget);
        }else{
            CustomSceneChange?.Invoke();
        }

        //Wait Transition Finish
        yield return new WaitForSeconds(1f);

        //Wait until transition to scene finish
        while(SceneManager.GetActiveScene().name != sceneTarget)
        {
            yield return null;
        }

        //Wait To End
        yield return new WaitForSeconds(.15f);
        TransitionPlaying = false;

        Destroy(transitionObj);
        OnEnd?.Invoke();
    }

    private IEnumerator EPlayTransition(TransitionStyle style, TransitionState end, Action OnStart = null, Action OnEnd = null)
    {
        //Setup Transition
        Transition transition = GetTransition(style);
        GameObject transitionObj = Instantiate(transition.transitionObject, transform);
        Animator transitionAnim = transitionObj.GetComponent<Animator>();

        string startAnim = end.ToString().ToLower();

        //Start Transition
        TransitionPlaying = true;

        //Play Start Transition
        OnStart?.Invoke();
        transitionAnim.Play(startAnim);

        //Wait Transition Finish
        yield return new WaitForSeconds(1f);

        //Wait To End
        yield return new WaitForSeconds(.15f);
        TransitionPlaying = false;
        Destroy(transitionObj);
        OnEnd?.Invoke();
    }

    private Transition GetTransition(TransitionStyle style) //Find the transition by the Style
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            if(transitions[i].style == style)
            {
                return transitions[i];
            }
        }
        return transitions[0];
    }
}

[System.Serializable]
public class Transition//Transition Object
{
    public GameObject transitionObject;
    public TransitionStyle style;
}

