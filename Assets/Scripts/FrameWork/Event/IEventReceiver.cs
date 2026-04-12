using UnityEngine;
namespace FrameWork.Event;

public interface IEventReceiver<TEvent> where TEvent : IEvent
{
    void OnEvent(TEvent e);

}

