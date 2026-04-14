using System.Drawing;
using FrameWork.Event;

namespace Events;

public readonly record struct SoftwareButtonClickedEvent : IEvent
{
    public readonly SoftwareType type;

    public SoftwareButtonClickedEvent(SoftwareType type)
    {
        this.type = type;
    }
}

