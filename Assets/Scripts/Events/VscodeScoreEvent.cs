using UnityEngine;
using FrameWork.Event;
namespace Events;

public readonly struct VscodeScoreEvent : IEvent
{
    public readonly int[] scores;

    public VscodeScoreEvent(int[] scores)
    {
        this.scores = scores;
    }
}
