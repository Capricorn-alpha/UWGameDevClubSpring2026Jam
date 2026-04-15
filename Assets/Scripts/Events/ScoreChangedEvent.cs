using FrameWork.Event;

namespace Events
{
    public readonly struct ScoreChangedEvent : IEvent
    {
        public readonly float ratingNormalized;
        public readonly int reviewsCount;

        public ScoreChangedEvent(float ratingNormalized, int reviewsCount)
        {
            this.ratingNormalized = ratingNormalized;
            this.reviewsCount = reviewsCount;
        }
    }
}
