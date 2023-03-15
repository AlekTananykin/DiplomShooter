using System;
using System.Text;
using TMPro;
using UnityEngine;

public class PlayerAccount : MonoBehaviour
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public void ToCreateAccount()
    {
        _progressBar.gameObject.SetActive(true);
        _progressBar.OnProgressFinish += OnCreateAccountProgressBarFinished;
        _progressBar.StartProgress();

        _playfabLogin.CreateAccount(Email, Username, Password);
    }

    public void ToSingnIn()
    {
        _progressBar.gameObject.SetActive(true);
        _progressBar.OnProgressFinish += OnSignInProgressBarFinished;
        _progressBar.StartProgress();

        _playfabLogin.SignIn(Username, Password);
    }

    public void ViewMessage(GameObject source, string message)
    {
        _messageSource = source;
        _messageSource.SetActive(false);
        _messagePannelView.gameObject.SetActive(true);

        _messagePannelView.ViewMessage(message);
    }

    public void ReturnFromMessagePanel() 
    {
        _messageSource.SetActive(true);
        _messagePannelView.gameObject.SetActive(false);

        _messageSource = null;
    }

    private void Start()
    {
        _messagePannelView.OnButtonClick += ReturnFromMessagePanel;

        ResetActiveStatus();

        _playfabLogin.OnRegistrationSuccess += OnCreateAccountSuccess;
        _playfabLogin.OnRegistrationError += OnCreateAccountError;

        _playfabLogin.OnSignInSuccess += OnSignInSuccess;
        _playfabLogin.OnSignInError += OnSignInError;

        _lobbyView.OnExitButtonClick += LobbyExit;
    }

    private void ResetActiveStatus()
    {
        _signInView.gameObject.SetActive(true);
        _createAccountView.gameObject.SetActive(false);
        _messagePannelView.gameObject.SetActive(false);
        _progressBar.gameObject.SetActive(false);
        _lobbyView.gameObject.SetActive(false);
    }

    private void LobbyExit()
    {
        ResetActiveStatus();
    }

    private void OnSignInError(string error)
    {
        StringBuilder message = new StringBuilder();
        message.AppendFormat(
            "Can't sign in user with username {0}. {1}",
            Username, error);

        _message = message.ToString();
        _progressBar.SetFastFinish();
        _isSignOk = false;
    }

    private void OnSignInSuccess(string obj)
    {
        _message = "Sign in success!";
        _progressBar.SetFastFinish();
        _isSignOk = true;
    }

    private void OnCreateAccountError(string palyfabError)
    {
        StringBuilder message = new StringBuilder();
        message.AppendFormat(
            "Can't registrate user with username {0}, Email {1}. {2}", 
            Username, Email, palyfabError);

        _message = message.ToString();
        _progressBar.SetFastFinish();
        _isSignOk = false;
    }

    private void OnCreateAccountSuccess(string palyfabError)
    {
        _message = "Create account success!";
        _progressBar.SetFastFinish();
        _isSignOk = true;
    }

    private void OnSignInProgressBarFinished()
    {
        _progressBar.OnProgressFinish -= OnSignInProgressBarFinished;

        _signInView.gameObject.SetActive(false);
        _createAccountView.gameObject.SetActive(false);
        _messagePannelView.gameObject.SetActive(false);
        _progressBar.gameObject.SetActive(false);

        if (_isSignOk)
        {
            _lobbyView.gameObject.SetActive(true);
            _lobbyView.ViewMessage();
        }
        else 
        {
            _messagePannelView.gameObject.SetActive(true);
            _messagePannelView.ViewMessage(_message);
        }
    }

    private void OnCreateAccountProgressBarFinished()
    {
        _progressBar.OnProgressFinish -= OnCreateAccountProgressBarFinished;
        _progressBar.gameObject.SetActive(false);
        ViewMessage(_createAccountView.gameObject, _message);
    }

    private void OnDestroy()
    {
        _messagePannelView.OnButtonClick -= ReturnFromMessagePanel;

        _playfabLogin.OnRegistrationSuccess -= OnCreateAccountSuccess;
        _playfabLogin.OnRegistrationError -= OnCreateAccountError;

        _playfabLogin.OnSignInSuccess -= OnSignInSuccess;
        _playfabLogin.OnSignInError -= OnSignInError;
    }

    [SerializeField]
    SignInView _signInView;

    [SerializeField]
    CreateAccountView _createAccountView;

    [SerializeField]
    Messager _messagePannelView;

    [SerializeField]
    private ProgressBar _progressBar;

    [SerializeField]
    private PlayFabSignIn _playfabLogin;

    [SerializeField]
    private LobbyView _lobbyView;


    private GameObject _messageSource;

    private string _message;
    private bool _isSignOk;
}