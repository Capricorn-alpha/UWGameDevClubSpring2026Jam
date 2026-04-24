using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReviewUI : MonoBehaviour
{
    [SerializeField] private Image UserProfile;
    [SerializeField] private TMP_Text UserName;
    [SerializeField] private TMP_Text ReviewContent;
    [SerializeField] private Image ReviewBackground;

    [SerializeField] private Sprite positiveBackground;
    [SerializeField] private Sprite negativeBackground;

    [SerializeField] private Sprite[] profileImages = new Sprite[5];

    public void SetupReview(ReviewData data)
    {
        UserName.text = data.UserName;
        ReviewContent.text = data.ReviewContent;

        ReviewBackground.sprite = data.condition.isPositive ? positiveBackground : negativeBackground;

        if(profileImages != null && profileImages.Length > 0)
        {
            int randomIndex = Random.Range(0, profileImages.Length);
            UserProfile.sprite = profileImages[randomIndex];
        }
    }
}