using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [SerializeField]private GameObject basePerson, interviewPerson, questionPerson;
    [SerializeField]private float queueSpacing;

    [SerializeField]private List<Person> queue = new List<Person>();

    private Person personInFront;

    public static QueueManager i;

    private void Awake()
    {
        i = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            AddPerson(null, basePerson);
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            RemoveFromQueue(0);
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            RemoveFromQueue(Random.Range(0, queue.Count));
        }
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

    public void AddQuestionPerson(string question, string confirm, string decline, System.Action confirmAction = null, System.Action declineAction = null)
    {
        GameObject person = AddPerson(questionPerson);

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
        
        if(c == null) return newPerson;

        PersonInfo info = newPerson.GetComponent<PersonInfo>();
        if(info != null)info.SetPerson(c);

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

            LeanTween.cancel(queue[i].gameObject);
            Person p = queue[i];
            float distance = Vector2.Distance(queue[i].gameObject.transform.position, new Vector2(0f - queueSpacing * i, queue[i].gameObject.transform.position.y));
            //queue[i].gameObject.LeanMove(new Vector2(0f - queueSpacing * i, queue[i].gameObject.transform.position.y), (.25f * Mathf.FloorToInt(distance / 5)));
            
            if(i == 0)
            {
                queue[i].gameObject.LeanMove(new Vector2(0f - queueSpacing * i, queue[i].gameObject.transform.position.y), (.075f * Mathf.FloorToInt(distance))).setOnComplete(()=>{
                    if(personInFront != queue[0])
                    {
                        queue[0].CallInFrontEvent();
                        personInFront = queue[0];
                    }
                });
            }
            else queue[i].gameObject.LeanMove(new Vector2(0f - queueSpacing * i, queue[i].gameObject.transform.position.y), (.075f * Mathf.FloorToInt(distance)));
        }
    }

    public void RemoveFromQueue(int who)
    {
        if(queue.Count <= 0) return;
        //Remover e mover a pessoa da fila
        LeanTween.cancel(queue[who].gameObject);
        float distance = Vector2.Distance(queue[who].gameObject.transform.position, new Vector2(15f, queue[who].gameObject.transform.position.y));
        Person save = this.queue[who];
        save.gameObject.LeanMove(new Vector2(15f, save.gameObject.transform.position.y), 1 + (.25f * Mathf.FloorToInt(distance / 5)) ).setOnComplete(()=>Destroy(save.gameObject));
        queue.RemoveAt(who);
        save.CallGoingAwayEvent();
        UpdateQueue();
    }

    public void RemoveFromQueueInterview()
    {
        if(queue.Count <= 0) return;
        //Remover e mover a pessoa da fila
        List<Person> removes = new List<Person>();
        foreach(Person q in queue)
        {
            if(q.GetComponent<InterviewPerson>() != null)
            {
                LeanTween.cancel(q.gameObject);
                float distance = Vector2.Distance(q.gameObject.transform.position, new Vector2(15f, q.gameObject.transform.position.y));
                Person save = q;
                save.gameObject.LeanMove(new Vector2(15f, save.gameObject.transform.position.y), 1 + (.25f * Mathf.FloorToInt(distance / 5)) ).setOnComplete(()=>Destroy(save.gameObject));
                removes.Add(save);
                save.CallGoingAwayEvent();
            }
        }

        for (int i = 0; i < removes.Count; i++) queue.Remove(removes[i]);

        UpdateQueue();
        
    }

    public void RemoveFromQueueCurriculum(Curriculum cur)
    {
        if(queue.Count <= 0) return;
        //Remover e mover a pessoa da fila
        for (int i = 0; i < queue.Count; i++)
        {
            if(queue[i].GetComponent<InterviewPerson>() != null)
            {
                if(queue[i].GetComponent<PersonInfo>().c == cur)
                {
                    LeanTween.cancel(queue[i].gameObject);
                    float distance = Vector2.Distance(queue[i].gameObject.transform.position, new Vector2(15f, queue[i].gameObject.transform.position.y));
                    Person save = queue[i];
                    save.gameObject.LeanMove(new Vector2(15f, save.gameObject.transform.position.y), 1 + (.25f * Mathf.FloorToInt(distance / 5)) ).setOnComplete(()=>Destroy(save.gameObject));
                    if(i == 0)save.CallGoingAwayEvent();
                    queue.Remove(save);
                }
            }
        }
        UpdateQueue();
    }
}
