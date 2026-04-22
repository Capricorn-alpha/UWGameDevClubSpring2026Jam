using UnityEngine;
using System.Collections.Generic;
using FrameWork.Event;
using Events;

public class PostListManager : MonoBehaviour, IEventReceiver<PostContainerReadyEvent>, IEventReceiver<DayChangedEvent>
{
    public PostDatabase database;
    public GameObject postPrefab;
    public RectTransform content;

    public int currentDay;
    public float currentScore = 0.6f;

    private int lastDay = -1;

    [SerializeField] private int maxPostsToShow = 3;

    private void OnEnable()
    {
        EventBus.Subscribe<PostContainerReadyEvent>(this);
        EventBus.Subscribe<DayChangedEvent>(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PostContainerReadyEvent>(this);
        EventBus.Unsubscribe<DayChangedEvent>(this);
    }

    public void OnEvent(PostContainerReadyEvent e)
    {
        content = e.content;
        GenerateTodayPosts(content);
    }

    public void OnEvent(DayChangedEvent e)
    {
        currentDay = e.day;
        Debug.Log("DayChangedEvent: " + currentDay);
        GenerateTodayPosts(content);
    }

    private void GenerateTodayPosts(Transform targetContent)
    {
        if(targetContent == null) return;

        if(isSameDay()) return;

        ClearPosts(targetContent);

        int currentGeneratedCount = 0; 

        foreach(PostData post in database.postDetails)
        {
            if(CanShowPost(post))
            {
                GameObject newPost = Instantiate(postPrefab, targetContent);

                PostUIController controller = newPost.GetComponent<PostUIController>();

                if(controller != null)
                {
                    controller.SetupPostDetails(post);
                }

                currentGeneratedCount++; 

                if (currentGeneratedCount >= maxPostsToShow)
                {
                    break; 
                }
            }
        }

        lastDay = currentDay;
    }

    private void ClearPosts(Transform targetContent)
    {
        
        if (targetContent == null) return;

        for (int i = targetContent.childCount - 1; i >= 0; i--)
        {
            Destroy(targetContent.GetChild(i).gameObject);
        }
    }

    private bool isSameDay()
    {
        Debug.Log("lastDay: " + lastDay + " currentDay: " + currentDay);
        return lastDay == currentDay;
    }

    private bool CanShowPost(PostData post)
    {
        bool isDayValid = currentDay >= post.postCondition.minDays && currentDay <= post.postCondition.maxDays;

        bool isScoreValid = currentScore >= post.postCondition.minScore && currentScore <= post.postCondition.maxScore;

        return isDayValid && isScoreValid;
    }
}
