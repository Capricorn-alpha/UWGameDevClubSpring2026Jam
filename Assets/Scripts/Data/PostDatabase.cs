using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class PostCondition
{
    [Header("Effective days")]
    public int minDays = 1;
    public int maxDays = 3;

    [Header("Effective score")]
    public float minScore = 0.5f;
    public float maxScore = 0.9f;

    [Header("Whether the bug fixed")]
    public bool isFixed = false;

}

[Serializable]
public class PostData
{
    [Header("Post ID")]
    public string postId;

    [Header("Post Condition")]
    public PostCondition postCondition;

    [Header("Post Info")]
    public Sprite userProfile;
    public string userName;

    [Header("Post Body")]
    public string postBody;

    [Header("Post Score")]
    public string postScore;
}

[CreateAssetMenu(menuName = "PostDatabase", fileName = "PostDatabase")]
public class PostDatabase : ScriptableObject
{
    public List<PostData> postDetails;
}
