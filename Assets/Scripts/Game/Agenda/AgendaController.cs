using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using FrameWork.Event;
using Events;

public class AgendaController : MonoBehaviour
{
    public GameObject postPrefab;
    private Queue<GameObject> postPrefabQueue;
    public TMP_Text dayCounter;
    public TMP_Text agendaText;
    private int day;
    private int bugs = 5;
    private bool dayStarted;

    public void Start()
    {
        day = 1;
        postPrefabQueue = new Queue<GameObject>();
        Instantiate(postPrefab);
        dayStarted = false;
        dayCounter.text = "Day " + day.ToString();
    }

    public void Update()
    {
        ShowAgenda();
    }

    // Add a new post prefab to the queue
    public void AddPostPrefab(GameObject postPrefab)
    {
        postPrefabQueue.Enqueue(postPrefab);
        agendaText.text += "- " + postPrefab.name + "\n";

        if (!dayStarted)
        {
            dayStarted = true;
        }
    }

    // Show all tasks in the agenda when the player clicks the vscode icon
    public void ShowAgenda()
    {
         agendaText.text = "Agenda for Day " + day.ToString() + ":\n";

        foreach (GameObject prefab in postPrefabQueue)
        {
            if (prefab == null) return;
            agendaText.text += "- " + prefab.name + "\n";
        }
    }

    // Reset the agenda for the next day
    public void ResetAgenda()
    {
        dayStarted = false;

        day++;
        dayCounter.text = "Day " + day.ToString();
        agendaText.text = "Agenda for Day " + day.ToString() + ":\n";
        postPrefabQueue.Clear();

        EventBus.Publish(new DayChangedEvent(day));
    }

    // Mark the task as completed
    public void CompleteTask()
    {
        agendaText.text = agendaText.text.Replace("- " + postPrefabQueue.Dequeue().name + "\n", "");
    }

    // Mark all agenda as completed
    public void CompleteAgenda()
    {
        if (bugs == 0)
        {
            agendaText.text = "Agenda for Day " + day.ToString() + ":\n- All tasks completed!";
        }
    }
}
