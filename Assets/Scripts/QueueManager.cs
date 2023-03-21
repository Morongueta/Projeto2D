using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [SerializeField]private GameObject basePerson;
    [SerializeField]private float queueSpacing;

    [SerializeField]private List<Person> queue = new List<Person>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            AddPerson();
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

    private void AddPerson()
    {
        GameObject newPerson = Instantiate(basePerson, new Vector2(-15f, basePerson.transform.position.y), Quaternion.identity);
        queue.Add(newPerson.GetComponent<Person>());
        UpdateQueue();
    }

    private void UpdateQueue()
    {
        //Mover a fila toda
        for (int i = 0; i < queue.Count; i++)
        {
            LeanTween.cancel(queue[i].gameObject);
            float distance = Vector2.Distance(queue[i].gameObject.transform.position, new Vector2(0f - queueSpacing * i, queue[i].gameObject.transform.position.y));
            queue[i].gameObject.LeanMove(new Vector2(0f - queueSpacing * i, queue[i].gameObject.transform.position.y), 1 + (.25f * Mathf.FloorToInt(distance / 5)));
        }
    }

    private void RemoveFromQueue(int who)
    {
        if(queue.Count <= 0) return;
        //Remover e mover a pessoa da fila
        LeanTween.cancel(queue[who].gameObject);
        float distance = Vector2.Distance(queue[who].gameObject.transform.position, new Vector2(15f, queue[who].gameObject.transform.position.y));
        Person save = this.queue[who];
        save.gameObject.LeanMove(new Vector2(15f, save.gameObject.transform.position.y), 1 + (.25f * Mathf.FloorToInt(distance / 5)) ).setOnComplete(()=>Destroy(save.gameObject));
        queue.RemoveAt(who);
        UpdateQueue();
    }
}
