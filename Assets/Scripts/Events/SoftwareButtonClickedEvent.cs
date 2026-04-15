using FrameWork.Event;

namespace Events;

public readonly struct SoftwareButtonClickedEvent : IEvent
{
    public readonly SoftwareType type;

    public SoftwareButtonClickedEvent(SoftwareType type)
    {
        this.type = type;
    }
}

