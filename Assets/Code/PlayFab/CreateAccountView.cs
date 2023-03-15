
using TMPro;
using UnityEngine;

public class CreateAccountView : MonoBehaviour
{
    public void OnCreateAccount()
    {
        _account.Username = _username.text;
        _account.Password = _passwordInput.text;
        _account.Email = _email.text;
        _account.ToCreateAccount();
    }

    [SerializeField]
    private PlayerAccount _account;

    [SerializeField]
    private TMP_InputField _email;

    [SerializeField]
    private TMP_InputField _username;

    [SerializeField]
    private TMP_InputField _passwordInput;
}
