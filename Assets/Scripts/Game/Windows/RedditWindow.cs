using UnityEngine;

public class RedditWindow : WindowBase
{
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject postPrefab;

    [SerializeField] private int postCount = 3;

    private void Start()
    {
        if (postCount > 0)
        {
            SpawnPosts(postCount);
        }
    }

    private void ClearPosts()
    {
        if (content == null) return;

        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }

    private void SpawnPosts(int postCount){
        if (postPrefab == null || content == null)
        {
            Debug.Log("Post prefab or content is not assigned");
            return;
        }

        for (int i = 0; i < postCount; i++)
        {
            var post = Instantiate(postPrefab, content);
            post.name = $"Post_{i+1}";
        }
    }

    public void ClearAndSpawnPosts()
    {
        ClearPosts();
        SpawnPosts(postCount);
    }
}
