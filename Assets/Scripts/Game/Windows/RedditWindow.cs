using UnityEngine;
using FrameWork.Event;
using Events;

public class RedditWindow : WindowBase
{
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject postPrefab;

    private void OnEnable()
    {
        // 只需要向外广播自己的 content 即可
        if(content != null)
        {
            EventBus.Publish(new PostContainerReadyEvent(content));
        }
    }

    // private void SpawnPosts(int postCount){
    //     if (postPrefab == null || content == null)
    //     {
    //         Debug.Log("Post prefab or content is not assigned");
    //         return;
    //     }

    //     for (int i = 0; i < postCount; i++)
    //     {
    //         var post = Instantiate(postPrefab, content);
    //         post.name = $"Post_{i+1}";
    //     }
    // }
}
