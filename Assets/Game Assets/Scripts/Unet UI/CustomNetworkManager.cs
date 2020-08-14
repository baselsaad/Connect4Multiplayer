using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
	private float nextRefreshTime;
	
	public void StartHosting(string nameofroom)
	{
		StartMatchMaker();
		matchMaker.CreateMatch(nameofroom, 2, true, "", "", "", 0, 0, OnMatchCreated);
		/*
		if (MenusScript.nameOfRomTxt.text == "" || MenusScript.nameOfRomTxt.text==null)
        {
			matchMaker.CreateMatch("Default", 4, true, "", "", "", 0, 0, OnMatchCreated);

		}
        else
        {
			
			matchMaker.CreateMatch(MenusScript.nameOfRomTxt.text, 2, true, "", "", "", 0, 0, OnMatchCreated);

		}
	*/
	}



    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
		DestroyObject(GameObject.Find("CustomNetworkManager"));
		DestroyObject(GameObject.Find("GameLogicHolderMulti(Clone)"));
		Application.LoadLevel("GameWorld");

	}


    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
		DestroyObject(GameObject.Find("CustomNetworkManager"));
		DestroyObject(GameObject.Find("GameLogicHolderMulti(Clone)"));
		Application.LoadLevel("GameWorld");
	}
    private void OnMatchCreated(bool success, string extendedinfo, MatchInfo responsedata)
	{
		base.StartHost(responsedata);
	
		RefreshMatches();
		
	}

	private void Update()
	{
		if (Time.time >= nextRefreshTime)
		{
			RefreshMatches();
		}
	}

	private void RefreshMatches()
	{
		nextRefreshTime = Time.time + 5f;

		if (matchMaker == null)
			StartMatchMaker();

		matchMaker.ListMatches(0, 10, "", true, 0, 0, HandleListMatchesComplete);
	}

	private void HandleListMatchesComplete(bool success, 
		string extendedinfo, 
		List<MatchInfoSnapshot> responsedata)
	{
		AvailableMatchesList.HandleNewMatchList(responsedata);
	}

	public void JoinMatch(MatchInfoSnapshot match)
	{
		if (matchMaker == null)
			StartMatchMaker();

		matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, HandleJoinedMatch);
		
	}


	public void JoinMatchs(MatchInfoSnapshot match,string matchname)
	{
		if (matchMaker == null)
			StartMatchMaker();

		matchMaker.JoinMatch(match.networkId, "", matchname, "", 0, 0, HandleJoinedMatch);

	}

	private void HandleJoinedMatch(bool success, string extendedinfo, MatchInfo responsedata)
	{
		StartClient(responsedata);
	}
}