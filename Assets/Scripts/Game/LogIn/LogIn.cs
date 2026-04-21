using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LogIn : MonoBehaviour
{
    [SerializeField] private Button logInButton;
    [SerializeField] private Button powerButton;

    [SerializeField] private GameObject LogInScreen;
    private bool logInIsClicked;
    private bool powerIsClicked;

    public void Start()
    {
        LogInScreen.SetActive(true);

        logInButton.onClick.AddListener(() => logInIsClicked = true);
        powerButton.onClick.AddListener(() => powerIsClicked = true);
    }

    public void Update()
    {
        if (logInIsClicked)
        {
            LogInScreen.SetActive(false);

            logInIsClicked = false;
        }

        if (powerIsClicked)
        {
            LogInScreen.SetActive(true);

            powerIsClicked = false;
        }
    }

    public bool LogInIsClicked()
    {
        return logInIsClicked;
    }
}
