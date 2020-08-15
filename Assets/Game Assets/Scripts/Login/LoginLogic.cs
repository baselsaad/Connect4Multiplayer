using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginLogic : MonoBehaviour
{

    public GameObject LoginUI, SignupUI, ErrorUI;

    public TMP_Text inputUserNameorEmailLogin, inputPasswordLogin , inputUsernameSignup, inputEmailSignup, inputPasswordSignup,errorMessage;

    public string usernameKey, emailkey, passwordKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void LoginClicked()
    {
        if (PlayerPrefs.GetString(emailkey).Equals(inputUserNameorEmailLogin.text) || PlayerPrefs.GetString(usernameKey).Equals(inputUserNameorEmailLogin.text))
        {
            if (PlayerPrefs.GetString(passwordKey).Equals(inputPasswordLogin.text)) { SceneManager.LoadScene("GameWorld");   }
            else { LoginUI.SetActive(false); ErrorUI.SetActive(true); errorMessage.text = "Your Password or Email is not Correct!"; errorMessage.fontSize = 33; }
        }
        else
        {
            LoginUI.SetActive(false); ErrorUI.SetActive(true); errorMessage.text = "Your Password or Email is not Correct!"; errorMessage.fontSize = 33;
        }
    }

    public void SignupClicked()
    {

        if (inputPasswordSignup.text.Equals(null) || inputEmailSignup.text.Equals(null) || inputUsernameSignup.text.Equals(null) || inputPasswordSignup.text.Equals("") || inputEmailSignup.text.Equals("") || inputUsernameSignup.text.Equals(""))
        {
            SignupUI.SetActive(false); ErrorUI.SetActive(true); errorMessage.text = "you have to enter all the gaps!"; errorMessage.fontSize = 33;
        }
        else
        {
            if (inputPasswordSignup.text.Length <= 6)
            {
                SignupUI.SetActive(false); ErrorUI.SetActive(true); errorMessage.text = "Your password must be greater than 6 letters!"; errorMessage.fontSize = 28;
            }
            else
            {

                PlayerPrefs.SetString(emailkey, inputEmailSignup.text);
                PlayerPrefs.SetString(usernameKey, inputUsernameSignup.text);
                PlayerPrefs.SetString(passwordKey, inputPasswordSignup.text);
                SceneManager.LoadScene("GameWorld");
            }
        }

     
    }

    public void OkClicked()
    {
        LoginUI.SetActive(true);
        ErrorUI.SetActive(false);
        SignupUI.SetActive(false); 
    }


    public void RegisterClicked()
    {
        LoginUI.SetActive(false);
        ErrorUI.SetActive(false);
        SignupUI.SetActive(true);
    }
}
