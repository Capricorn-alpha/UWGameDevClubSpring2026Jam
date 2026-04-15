using FrameWork.Event;

namespace Events;

public readonly struct AfterReplyCommentEvent: IEvent
{
    public readonly int optionIndex;

    public AfterReplyCommentEvent(int optionIndex) => this.optionIndex = optionIndex;
}
