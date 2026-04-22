using UnityEngine;
using FrameWork.Event;

namespace Events;

public readonly struct PostContainerReadyEvent : IEvent
{
    public readonly RectTransform content;

    public PostContainerReadyEvent(RectTransform content)
    {
        this.content = content;
    }
}

