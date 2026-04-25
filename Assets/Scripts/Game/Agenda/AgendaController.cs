using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using FrameWork.Event;
using Events;

public class AgendaController : MonoBehaviour
{
    public TMP_Text dayCounter;
    private int day;
    private int bugs = 5;
    private bool dayStarted;

    public void Start()
    {
        day = 1;
        dayStarted = false;
        dayCounter.text = "Day " + day.ToString();
    }

    // Reset the agenda for the next day
    public void ResetAgenda()
    {
        dayStarted = false;

        day++;
        dayCounter.text = "Day " + day.ToString();

        EventBus.Publish(new DayChangedEvent(day));
    }
}
