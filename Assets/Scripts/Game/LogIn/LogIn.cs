using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LogIn : MonoBehaviour
{
    [SerializeField] private Button logInButton;
    [SerializeField] private Button powerButton;
    [SerializeField] private Button logOutButton;

    [SerializeField] private GameObject LogInScreen;
    [SerializeField] private GameObject LogOutScreen;
    [SerializeField] private AgendaController agendaController;
    private bool logInIsClicked;
    private bool logOutIsClicked;
    private bool powerIsClicked;

    public void Start()
    {
        LogInScreen.SetActive(true);
        LogOutScreen.SetActive(false);

        logInButton.onClick.AddListener(() => logInIsClicked = true);
        powerButton.onClick.AddListener(() => powerIsClicked = true);
        logOutButton.onClick.AddListener(() => {logOutIsClicked = true; agendaController.ResetAgenda();});
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
            LogOutScreen.SetActive(true);

            powerIsClicked = false;
        }

        if (logOutIsClicked)
        {
            LogOutScreen.SetActive(false);
            LogInScreen.SetActive(true);

            logOutIsClicked = false;
        }
    }

    public bool LogInIsClicked()
    {
        return logInIsClicked;
    }
}
