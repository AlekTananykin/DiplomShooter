using TMPro;
using UnityEngine;


public class SignInView : MonoBehaviour
{
    public void OnSignIn()
    {
        _account.Username = _username.text;
        _account.Password = _passwordInput.text;

        _account.ToSingnIn();
    }

    [SerializeField]
    private PlayerAccount _account;

    [SerializeField]
    private TMP_InputField _username;

    [SerializeField]
    private TMP_InputField _passwordInput;
}
