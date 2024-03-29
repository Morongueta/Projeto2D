using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]private GameObject tutorialStepOne;
    [SerializeField]private GameObject tutorialStepTwo;

    [SerializeField]private SpriteRenderer[] blinkSprites;

    [SerializeField]private Curriculum bossCurriculum;

    [SerializeField]private bool alwaysRunTutorial;

    private bool waitFirstHiringFinish;

    private const string TUTORIAL_KEY = "TUTORIAL_KEY";

    public static TutorialManager i;

    private void Awake()
    {
        i = this;
    }

    private void Start() 
    {
        if(alwaysRunTutorial || PlayerPrefs.GetInt(TUTORIAL_KEY, 0) == 0)
            SpawnOne();
    }

    public void BlinkTutorial()
    {
        CameraManager.i.BlinkArrow();
        BlinkObjects();
    }

    public void BlinkObjects()
    {
        StartCoroutine(EBlink());
    }

    public IEnumerator EBlink()
    {
        float blinkTime = 20f;
        while(blinkTime > 0f)
        {
            blinkTime -= Time.deltaTime * 4f;

            for (int i = 0; i < blinkSprites.Length; i++)
            {
                blinkSprites[i].enabled = ((int)blinkTime % 2 == 0);

            }
            yield return null;
        }

        for (int i = 0; i < blinkSprites.Length; i++)
        {
            blinkSprites[i].enabled = true;
        }
    }

    public void SpawnOne() {
        TimeManager.i.timeIsRunning = false;
        EventController.i.eventIsOn = false;
        QueueManager.i.AddThisPerson(tutorialStepOne,bossCurriculum);  
    }


    public void SpawnTwo()
    {
        StartCoroutine(ETutorialStepTwo());
    }

    private IEnumerator ETutorialStepTwo()
    {
        while(HiringManager.i.hireState != HireState.NONE)
        {
            yield return null;
        }
        
        QueueManager.i.AddThisPerson(tutorialStepTwo,bossCurriculum);  

    }

    public void ResetTutorial()
    {
        CoexistenceManager.i.RemoveTutorial();
        SpawnOne();
    }

    public void EndTutorial()
    {
        TimeManager.i.timeIsRunning = true;
        CoexistenceManager.i.RemoveTutorial();
        PlayerPrefs.SetInt(TUTORIAL_KEY, 1);

        EventController.i.eventIsOn = true;

        TextBoxManager.i.HideTextBox();
    }
}
