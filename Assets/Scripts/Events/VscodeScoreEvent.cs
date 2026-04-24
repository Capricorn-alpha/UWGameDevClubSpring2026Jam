using UnityEngine;
using FrameWork.Event;
namespace Events;

public readonly struct VscodeScoreEvent : IEvent
{
    public readonly int[] counts;

    public VscodeScoreEvent(int[] counts)
    {
        this.counts = counts;
    }
}
