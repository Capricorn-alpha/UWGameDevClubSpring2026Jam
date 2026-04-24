using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class ReviewCondition
{   
    [Header("Effective days")]
    public int minDays = 1;
    public int maxDays = 3;

    [Header("Effective score")]
    public float minScore = 0.5f;
    public float maxScore = 0.9f;

    [Header("Whether the review is positive")]
    public bool isPositive = true;
}

[Serializable]
public class ReviewData
{
    [Header("Review Condition")]
    public ReviewCondition condition;

    [Header("Review Info")]
    public string UserName;
    
    [TextArea] public string ReviewContent;
}

[CreateAssetMenu(menuName = "ReviewDatabase", fileName = "ReviewDatabase")]
public class ReviewDatabase : ScriptableObject
{
    public List<ReviewData> reviewDetails;
}