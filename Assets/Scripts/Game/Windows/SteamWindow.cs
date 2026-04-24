using UnityEngine;
using TMPro;
using FrameWork.Event;
using Events;

public class SteamWindow : WindowBase, IEventReceiver<ScoreChangedEvent>
{
    [Header("Review")]
    [SerializeField] private TMP_Text reviewsStatusText;
    [SerializeField] private TMP_Text reviewsCountText;
    [SerializeField] private TMP_Text reviewsStatusInENG;
    [SerializeField] private TMP_Text reviewsCountInENG;

    [SerializeField] private RectTransform content;

    private void OnEnable()
    {
        EventBus.Subscribe<ScoreChangedEvent>(this);
        
        EventBus.Publish(new ReviewContainerReadyEvent(content));

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.RegisterSteamWindow();
            UpdateReviewUI(ScoreManager.Instance.RatingNormalized, ScoreManager.Instance.ReviewsCount);
        }
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ScoreChangedEvent>(this);
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.UnregisterSteamWindow();
    }

    public void OnEvent(ScoreChangedEvent e)
    {
        UpdateReviewUI(e.ratingNormalized, e.reviewsCount);
    }

    private void UpdateReviewUI(float ratingNormalized, int reviewsCount)
    {
        string status = GetReviewStatus(ratingNormalized);
        string countText = $"({reviewsCount})";

        if (reviewsStatusText != null) reviewsStatusText.text = status;
        if (reviewsCountText != null) reviewsCountText.text = countText;
        if (reviewsStatusInENG != null) reviewsStatusInENG.text = status;
        if (reviewsCountInENG != null) reviewsCountInENG.text = countText;
    }

    private static string GetReviewStatus(float score)
    {
        if (score >= 0.85f) return "Masterpiece";
        if (score >= 0.70f) return "Actually fun";
        if (score >= 0.55f) return "Mostly Positive";
        if (score >= 0.40f) return "This is okay";
        if (score >= 0.25f) return "Touch grass";
        return "Please go and get a job";
    }
}
