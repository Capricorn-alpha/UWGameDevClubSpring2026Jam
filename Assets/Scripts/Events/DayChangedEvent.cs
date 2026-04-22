using UnityEngine;
using FrameWork.Event;

namespace Events;

public readonly struct DayChangedEvent : IEvent
{
    public readonly int day;

    public DayChangedEvent(int day)
    {
        this.day = day;
    }
}
