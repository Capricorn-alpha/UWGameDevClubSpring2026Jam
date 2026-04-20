using System.Diagnostics.Contracts;
using UnityEngine;

public class LogIn : MonoBehaviour
{
    [SerializeField] private HoldButton LogInButton;
    [SerializeField] private HoldButton powerButton;

    [SerializeField] private GameObject LogInScreen;

    public void Start()
    {
        LogInScreen.SetActive(true);
    }

    public void Update()
    {
        if (LoggedIn())
        {
            LogInScreen.SetActive(false);
        }

        LogOut();
    }


    public bool LoggedIn()
    {
        return LogInButton.isPressed;
    }

    public void LogOut()
    {
        if (powerButton.isPressed)
        {
            LogInScreen.SetActive(true);
        }
    }
}
