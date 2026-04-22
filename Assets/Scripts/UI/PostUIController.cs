using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PostUIController : MonoBehaviour
{
    public Image userProfileImage;
    public TMP_Text userNameText;
    public TMP_Text postBodyText;
    public TMP_Text postScoreText;

    public void SetupPostDetails(PostData postData)
    {
        userProfileImage.sprite = postData.userProfile;
        userNameText.text = postData.userName;
        postBodyText.text = postData.postBody;
        postScoreText.text = postData.postScore;
    }
}
