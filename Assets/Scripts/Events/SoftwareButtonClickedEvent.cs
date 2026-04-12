using FrameWork.Event;

namespace FrameWork.Event;

public class SoftwareButtonClickedEvent : IEvent
{
    public SoftwareType SoftwareType { get; }

    public SoftwareButtonClickedEvent(SoftwareType softwareType)
    {
        SoftwareType = softwareType;
    }
}

