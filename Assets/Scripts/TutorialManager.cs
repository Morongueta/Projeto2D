using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]private GameObject tutorialStepOne;
    [SerializeField]private bool alwaysRunTutorial;

    private const string TUTORIAL_KEY = "TUTORIAL_KEY";

    private void Start() 
    {
        QueueManager.i.AddThisPerson(tutorialStepOne);
    }
}
