using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenusScript : MonoBehaviour
{

    public GameObject inputfieldofRoomname  , CreateButton, JoinButton , CreateRoomwithname , JoinGameWithName;
    public TMP_Text txtNameOfRoom;
    private MatchInfoSnapshot match;
    private List <MatchInfoSnapshot> matchsList;
    public GameObject menu;

  
    public void btnCreateRoomClicked()
    {
        inputfieldofRoomname.SetActive(true);
        JoinButton.SetActive(false);
        CreateRoomwithname.SetActive(true);
        
    }

    private void Awake()
    {
        AvailableMatchesList.OnAvailableMatchesChanged += AvailableMatchesList_OnAvailableMatchesChanged;
    }

    private void AvailableMatchesList_OnAvailableMatchesChanged(List<MatchInfoSnapshot> matches)
    {
       
        CreateNewJoinGameButtons(matches);
    }
   

    private void CreateNewJoinGameButtons(List<MatchInfoSnapshot> matches)
    {
        matchsList = matches;
    }

    public void btnJoinGameClicked()
    {
        JoinGameWithName.SetActive(true);
        inputfieldofRoomname.SetActive(true);
        CreateRoomwithname.SetActive(false);
        CreateButton.SetActive(false);
        JoinButton.SetActive(false);
      
     
    }


    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            CreateButton.SetActive(true);
            CreateRoomwithname.SetActive(false);
            JoinButton.SetActive(true);
            JoinGameWithName.SetActive(false);
            inputfieldofRoomname.SetActive(false);
            menu.SetActive(true);
            NetworkManager.Shutdown();
            Application.LoadLevel("GameWorld");
        }
    }



    public void btnJoinGameWithName()
    {
        try
        {
            for (int i = 0; i <= matchsList.Count; i++)
            {
                if (matchsList[i].name.Equals(txtNameOfRoom.text))
                {
                    menu.SetActive(false);
                    Debug.Log("true : " + matchsList[i].name);
                    try
                    {
                        FindObjectOfType<CustomNetworkManager>().JoinMatch(matchsList[i]);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error" + e);
                    }

                }

            }
        }catch (Exception e)
        {
            Debug.Log("Error" + e);
        }
      


       // FindObjectOfType<CustomNetworkManager>().JoinMatch(match);
    }


    public void btnCreatwithRoomNameClicked()
    {
        // CustomNetworkManager custom = new CustomNetworkManager();
        // custom.StartHosting(nameOfRomTxt.text);
      
        
        menu.SetActive(false);
        try
        {
            FindObjectOfType<CustomNetworkManager>().StartHosting(txtNameOfRoom.text);
            Debug.Log(txtNameOfRoom.text);
        }
        catch(Exception e)
        {
            Debug.Log("Error: " + e);
        }
        

    }

  

}
