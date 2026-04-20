using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LogInText : MonoBehaviour
{
    public TMP_InputField account;
    public TMP_InputField password;

    public void OnLoginButtonClicked() 
    {
        string usernameStr = account.text;
        string passwordStr = password.text;

        Debug.Log("Account: " + usernameStr + "\n Password: " + passwordStr);
    }
}
