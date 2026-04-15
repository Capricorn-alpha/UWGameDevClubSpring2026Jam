using UnityEngine;
using TMPro;

public class AgendaController : MonoBehaviour
{
    public GameObject postPrefab;
    private Queue<GameObject> postPrefabQueue;
    public TextMeshProUGUI dayCounter;
    public TextMeshProUGUI agendaText;
    private int day;
    private int bugs = 5;
    private bool dayStarted;
    private bool agendaCompleted;

    public void Start()
    {
        day = 1;

        postPrefabQueue = new Queue<GameObject>();
        Instantiate(postPrefab);

        dayStarted = false;
        agendaCompleted = false;

        dayCounter.text = "Day " + day.ToString();
    }

    public void Update()
    {
        ShowAgenda();

        if (dayStarted && agendaCompleted)
        {
            ResetAgenda();
        }
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
            agendaText.text += "- " + prefab.name + "\n";
        }
    }

    // Reset the agenda for the next day
    public void ResetAgenda()
    {
        if (dayStarted && agendaCompleted)
        {
            dayStarted = false;
            agendaCompleted = false;

            day++;
            dayCounter.text = "Day " + day.ToString();
            agendaText.text = "Agenda for Day " + day.ToString() + ":\n";
            postPrefabQueue.Clear();
        }
    }

    // Mark the task as completed
    public void CompleteTask()
    {
        agendaText.text = agendaText.text.Replace("- " + postPrefabQueue.Dequeue().name + "\n", "");
    }

    public void CompleteAgenda()
    {
        if (bugs == 0)
        {
            agendaText.text = "Agenda for Day " + day.ToString() + ":\n- All tasks completed!";
            agendaCompleted = true;
        }
    }
}
