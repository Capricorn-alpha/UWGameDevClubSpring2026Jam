using UnityEngine;
using System.Collections.Generic;
using FrameWork.Event;
using Events;

public class ReviewListManager : MonoBehaviour, IEventReceiver<ReviewContainerReadyEvent>, IEventReceiver<DayChangedEvent>, IEventReceiver<ScoreChangedEvent>
{
    public ReviewDatabase database;
    public GameObject reviewPrefab;
    public RectTransform content;

    public int currentDay = 1;
    public float currentScore = 0.6f;

    private int lastDay = -1;
    private float lastScore = -1f;

    [SerializeField] private int maxReviewsToShow = 5;

    private void OnEnable()
    {
        EventBus.Subscribe<ReviewContainerReadyEvent>(this);
        EventBus.Subscribe<DayChangedEvent>(this);
        EventBus.Subscribe<ScoreChangedEvent>(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ReviewContainerReadyEvent>(this);
        EventBus.Unsubscribe<DayChangedEvent>(this);
        EventBus.Unsubscribe<ScoreChangedEvent>(this);
    }

    public void OnEvent(ReviewContainerReadyEvent e)
    {
        Debug.Log("Something is ready");
        content = e.content;
        GenerateReviews(content);
    }

    public void OnEvent(DayChangedEvent e)
    {
        currentDay = e.day;
        Debug.Log("ReviewManager - DayChanged: " + currentDay);
        GenerateReviews(content);
    }

    public void OnEvent(ScoreChangedEvent e)
    {
        currentScore = e.ratingNormalized;
        Debug.Log("ReviewManager - ScoreChanged: " + currentScore);
        GenerateReviews(content);
    }

    private void GenerateReviews(Transform targetContent)
    {
        if(targetContent == null) return;

        if(isSameState()) return;

        ClearReviews(targetContent);

        int currentGeneratedCount = 0; 

        foreach(ReviewData review in database.reviewDetails)
        {
            if(CanShowReview(review))
            {
                GameObject newReview = Instantiate(reviewPrefab, targetContent);

                ReviewUI uiController = newReview.GetComponent<ReviewUI>();

                if(uiController != null)
                {
                    uiController.SetupReview(review);
                }

                currentGeneratedCount++; 

                if (currentGeneratedCount >= maxReviewsToShow)
                {
                    break; 
                }
            }
        }

        lastDay = currentDay;
        lastScore = currentScore;
    }

    private void ClearReviews(Transform targetContent)
    {
        if (targetContent == null) return;

        for (int i = targetContent.childCount - 1; i >= 0; i--)
        {   
            RectTransform child = targetContent.GetChild(i) as RectTransform;

            if(child.GetComponent<ReviewUI>() != null)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private bool isSameState()
    {
        return lastDay == currentDay && Mathf.Approximately(lastScore, currentScore);
    }

    private bool CanShowReview(ReviewData review)
    {
        bool isDayValid = currentDay >= review.condition.minDays && currentDay <= review.condition.maxDays;
        bool isScoreValid = currentScore >= review.condition.minScore && currentScore <= review.condition.maxScore;

        return isDayValid && isScoreValid;
    }
}
