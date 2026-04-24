using FrameWork.Event;
using UnityEngine;

namespace Events;

public readonly struct ReviewContainerReadyEvent : IEvent
{
    public readonly RectTransform content;

    public ReviewContainerReadyEvent(RectTransform content)
    {
        this.content = content;
    }
}