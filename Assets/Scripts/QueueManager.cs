using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [SerializeField]private GameObject basePerson, interviewPerson, questionPerson, reportPerson;
    [SerializeField]private float queueSpacing;

    [SerializeField]private List<Person> queue = new List<Person>();

    private Person personInFront;

    public static QueueManager i;

    private void Awake()
    {
        i = this;
    }

    public void AddHiringPerson(GameObject[] papers, bool delayed = false, float timeBtwPerson = .15f)
    {
        if(delayed)
        {
            StartCoroutine(EAddHiringPerson(papers, timeBtwPerson));
            return;
        }
        for (int i = 0; i < papers.Length; i++)
        {
            AddPerson(papers[i].GetComponent<Curriculum>(), interviewPerson);
        }
    }

    public void AddQuestionPerson(string question, string confirm, string decline, System.Action confirmAction = null, System.Action declineAction = null, Curriculum c = null)
    {
        GameObject person = AddPerson(c, questionPerson);

        PersonQuestion personQuestion = person.GetComponent<PersonQuestion>();

        if(personQuestion != null)
        {
            personQuestion.question = question;
            personQuestion.confirm = confirm;
            personQuestion.decline = decline;

            personQuestion.confirmAction += confirmAction;
            personQuestion.declineAction += declineAction;
        }
    }

    public void AddReportPerson(string reportText, string reportConfirm, System.Action confirmAction = null)
    {
        GameObject person = AddPerson(reportPerson);

        PersonReport personReport = person.GetComponent<PersonReport>();

        if(personReport != null)
        {
            personReport.reportText = reportText;
            personReport.reportConfirm = reportConfirm;

            personReport.confirmAction += confirmAction;
        }
    }

    public void AddThisPerson(GameObject person, Curriculum c = null)
    {
        GameObject p = AddPerson(c, person);
    }

    private IEnumerator EAddHiringPerson(GameObject[] papers, float delay)
    {
        int i = 0;
        while(i < papers.Length)
        {
            AddPerson(papers[i].GetComponent<Curriculum>(), interviewPerson);
            yield return new WaitForSeconds(delay);
            i++;
        }
    }

    private GameObject AddPerson(Curriculum c, GameObject person)
    {
        GameObject newPerson = Instantiate(person, new Vector2((-queueSpacing * queue.Count) - 15f, person.transform.position.y), Quaternion.identity);
        queue.Add(newPerson.GetComponent<Person>());
        UpdateQueue();
        
        if(c == null)
        {
            CurriculumData data = new CurriculumData();
            PersonData randomPerson = InformationDatabase.i.GeneratePerson();
            data.Store(randomPerson);

            PersonInfo information = newPerson.GetComponent<PersonInfo>();
            if(information != null)information.SetPerson(data);

            return newPerson;
        }

        PersonInfo info = newPerson.GetComponent<PersonInfo>();
        if(info != null)info.SetPerson(c.curriculumData);

        return newPerson;
    }

    private GameObject AddPerson(GameObject person)
    {
        return AddPerson(null, person);
    }

    private void UpdateQueue(bool ignoreTheFirst = false)
    {
        //Mover a fila toda
        for (int i = 0; i < queue.Count; i++)
        {
            if(ignoreTheFirst && i == 0) return;

            int index = i;

            LeanTween.cancel(queue[i].gameObject);
            Person p = queue[i];
            float distance = Vector2.Distance(queue[i].gameObject.transform.position, new Vector2(0f - queueSpacing * i, queue[i].gameObject.transform.position.y));
            //queue[i].gameObject.LeanMove(new Vector2(0f - queueSpacing * i, queue[i].gameObject.transform.position.y), (.25f * Mathf.FloorToInt(distance / 5)));

            p.walking = true;

            if (i == 0)
            {
                queue[i].gameObject.LeanMoveX(0f - queueSpacing * i, (.075f * Mathf.FloorToInt(distance))).setOnComplete(() =>
                {
                    p.walking = false;
                    if (personInFront != queue[0])
                    {
                        queue[0].CallInFrontEvent();
                        personInFront = queue[0];
                    }
                });
            }
            else queue[i].gameObject.LeanMoveX(0f - queueSpacing * i, (.075f * Mathf.FloorToInt(distance))).setOnComplete(() =>
            {
                p.walking = false;
            }) ;
        }
    }

    public void RemoveFromQueue(int who, bool autoUpdate = true)
    {
        if(queue.Count <= 0) return;
        //Remover e mover a pessoa da fila
        LeanTween.cancel(queue[who].gameObject);
        float distance = Vector2.Distance(queue[who].gameObject.transform.position, new Vector2(15f, queue[who].gameObject.transform.position.y));
        Person save = this.queue[who];
        save.walking = true;
        save.gameObject.LeanMoveX(15f, 1 + (.25f * Mathf.FloorToInt(distance / 5)) ).setOnComplete(()=>Destroy(save.gameObject));
        queue.RemoveAt(who);
        save.CallGoingAwayEvent();
        if(autoUpdate)UpdateQueue();
    }

    public void RemoveFromQueueInterview()
    {
        //Remover e mover a pessoa da fila
        List<Person> removes = new List<Person>();

        for (int i = 0; i < queue.Count; )
        {
            Person save = this.queue[i];
            if (save.GetComponent<InterviewPerson>() != null)
            {
                RemoveFromQueue(i, false);
            }else{
                i++;
            }
        }
        UpdateQueue();
    }

    public void RemoveFromQueueCurriculum(CurriculumData cur)
    {
        if(queue.Count <= 0) return;
        //Remover e mover a pessoa da fila
        for (int i = 0; i < queue.Count; i++)
        {
            if(queue[i].GetComponent<InterviewPerson>() != null)
            {
                if(queue[i].GetComponent<PersonInfo>().c.personName == cur.personName)
                {
                    LeanTween.cancel(queue[i].gameObject);
                    float distance = Vector2.Distance(queue[i].gameObject.transform.position, new Vector2(15f, queue[i].gameObject.transform.position.y));
                    Person save = queue[i];
                    save.walking = true;
                    save.gameObject.LeanMoveX(15f, 1 + (.25f * Mathf.FloorToInt(distance / 5)) ).setOnComplete(()=>Destroy(save.gameObject));
                    if(i == 0)save.CallGoingAwayEvent();
                    queue.Remove(save);
                }
            }
        }
        UpdateQueue();
    }
}
