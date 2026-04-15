using UnityEngine;
using TMPro;
using FrameWork.Event;
using Events;

public class SteamWindow : WindowBase, IEventReceiver<AfterReplyCommentEvent>
{   
    [Header("Review")]
    [SerializeField] private TMP_Text reviewsStatusText;
    [SerializeField] private TMP_Text reviewsCountText;
    [SerializeField] private TMP_Text reviewsStatusInENG;
    [SerializeField] private TMP_Text reviewsCountInENG;

    [Header("Score Model")]
    [Range(0f, 1f)]
    [SerializeField] private float ratingNormalized = 0.75f;
    [SerializeField] private int reviewsCount = 456;
    [Header("Tuning")]
    [SerializeField] private float politeReplyDelta = 0.03f;
    [SerializeField] private float rudeReplyDelta = -0.05f;
    [SerializeField] private int reviewCountDeltaPerAction = 8;

    private void OnEnable()
    {
        EventBus.Subscribe<AfterReplyCommentEvent>(this);
        Debug.Log($"[SteamWindow] Subscribed. Initial score={ratingNormalized:F3}, reviews={reviewsCount}", this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<AfterReplyCommentEvent>(this);
    }

    public void OnEvent(AfterReplyCommentEvent e)
    {
        float beforeScore = ratingNormalized;
        int beforeReviews = reviewsCount;

        if(e.optionIndex == 0)
        {
            ApplyReviewChange(politeReplyDelta, reviewCountDeltaPerAction);
        }
        else if (e.optionIndex == 2)
        {
            ApplyReviewChange(rudeReplyDelta, reviewCountDeltaPerAction);
        }

        Debug.Log(
            $"[SteamWindow] Reply option={e.optionIndex} | score {beforeScore:F3} -> {ratingNormalized:F3} | reviews {beforeReviews} -> {reviewsCount}",
            this
        );
    }

    private void ApplyReviewChange(float scoreDelta, int countDelta)
    {
        ratingNormalized = Mathf.Clamp01(ratingNormalized + scoreDelta);
        reviewsCount = Mathf.Max(0, reviewsCount + countDelta);
        UpdateReviewUI();
    }

    private void UpdateReviewUI()
    {
        string status = GetReviewStatus(ratingNormalized);
        string countText = $"({reviewsCount})";

        if(reviewsStatusText != null) reviewsStatusText.text = status;
        if(reviewsCountText != null) reviewsCountText.text = countText;
        if(reviewsStatusInENG != null) reviewsStatusInENG.text = status;
        if(reviewsCountInENG != null) reviewsCountInENG.text = countText;

    }

    private static string GetReviewStatus(float score)
    {
        if(score >= 0.85f) return "Overwhelmingly Positive";
        if(score >= 0.70f) return "Very Positive";
        if(score >= 0.55f) return "Mostly Positive";
        if(score >= 0.40f) return "Mixed";
        if(score >= 0.25f) return "Mostly Negative";
        return "Overwhelmingly Negative";
    }
}
