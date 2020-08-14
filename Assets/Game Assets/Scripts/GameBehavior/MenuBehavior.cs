using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuBehavior : MonoBehaviour
{
    public GameLogic gameLogic;
    public GameObject mainMenuBody;
    public Canvas mainMenu;
    public Canvas difficultMenu;
    public GameObject Settings; 




    public void btnPlayClicked()
    {
        mainMenu.enabled = false;
        difficultMenu.enabled = true;
    }

    public void btnExitClicked()
    {
        Application.Quit();
    }

    public void btnEasyClicked()
    {
        mainMenu.enabled = true;
        difficultMenu.enabled = false;
        mainMenuBody.SetActive(false);


        gameLogic.StartGame(4);
    }
    public void btnHardClicked()
    {
        mainMenu.enabled = true;
        difficultMenu.enabled = false;
        mainMenuBody.SetActive(false);

        gameLogic.StartGame(6);
    }
    public void btnVeryHardClicked()
    {
        mainMenu.enabled = true;
        difficultMenu.enabled = false;
        mainMenuBody.SetActive(false);

        gameLogic.StartGame(8);
    }

    public void btnMainMenuClicked()
    {
        mainMenu.enabled = true;
        difficultMenu.enabled = false;
    }

    public void btnBackCLicked()
    {
        mainMenu.enabled = true;
        difficultMenu.enabled = false;
        Settings.SetActive(false); 
    }

    public void btnSettings()
    {
        mainMenu.enabled = false;
        difficultMenu.enabled = false;
        Settings.SetActive(true);
    }

	public void btnMultiPLayer (){
		mainMenu.enabled = true;
		difficultMenu.enabled = false;
		mainMenuBody.SetActive(true);
		SceneManager.LoadScene ("multi");
		

	}

    void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (gameLogic.isPlaying)
            {
                gameLogic.MainMenu();
                mainMenu.enabled = true;
                difficultMenu.enabled = false;
                mainMenuBody.SetActive(true);
            }
            else
            {
                gameLogic.ResumeGame();
                difficultMenu.enabled = false;
                mainMenu.enabled = false;
                mainMenuBody.SetActive(false);
            }
        }
    }
}
