using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{

	[SyncVar(hook = "OnTurnChange")]
	public bool isTurn = false;

	[SyncVar(hook = "UpdateTimeDisplay")]
	public float timers = 10f;

	

	public PlayerController controller;
	
	public HitboxBehaviorMulti Colms1, Colms2, Colms3, Colms4, Colms5, Colms6, Colms7;
	public GameLogicMulti gameLogic;

	[SyncVar]
	public bool ready = false;

	// Use this for initialization
	void Start()
	{
		controller.OnPlayerInput += OnPlayerInput;
		
	}

	
	// Update is called once per frame
	[Server]
	void Update()
	{
		if (isTurn)
		{
			timers -= Time.deltaTime;
			
			if (timers <= 0)
			{
				Debug.Log ("count to zeor");
				NetworkManager.Instance.AlterTurns();
				timers = 10f;
			}
		}
	}


   
    public override void OnStartClient()
	{
		DontDestroyOnLoad(this);

		base.OnStartClient();
		Debug.Log("Client Network Player start");
		StartPlayer();

		NetworkManager.Instance.RegisterNetworkPlayer(this);
	}

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		controller.SetupLocalPlayer();
	}

	[Server]
	public void StartPlayer()
	{
		ready = true;
	}

	public void StartGame()
	{
		TurnStart();
	}

	[Server]
	public void TurnStart()
	{
		
		isTurn = true;
		RpcTurnStart();
	}

	[ClientRpc]
	void RpcTurnStart()
	{
		controller.TurnStart();
	}

	[Server]
	public void TurnEnd()
	{
		isTurn = false;
		RpcTurnEnd();
	}

	[ClientRpc]
	void RpcTurnEnd()
	{
		controller.TurnEnd();
	}

	public override void OnNetworkDestroy()
	{
		base.OnNetworkDestroy();
		NetworkManager.Instance.DeregisterNetworkPlayer(this);
	}

	public void OnTurnChange(bool turn)
	{
		if (isLocalPlayer)
		{
			//play turn sound 
		}
	}

	public void UpdateScore(int score)
	{
		Debug.Log ("score:"+score);
	}

	void OnPlayerInput(PlayerAction action, float amount)
	{
		if (action == PlayerAction.SHOOT)
		{
			CmdOnPlayerInput(action, amount);
		}
	}

	[Command]
	void CmdOnPlayerInput(PlayerAction action, float amount)
	{
		//Shoot bullets
		

		//Update score
		int myint =(int) Mathf.Round(amount);
		NetworkManager.Instance.UpdateScore(myint);
	}

	public void UpdateTimeDisplay(float curtime)
	{
		GameObject timerText = GameObject.FindWithTag("Timer");
		Text timer = timerText.GetComponent<Text> ();
		timer.text = Mathf.Round(curtime).ToString();
	}
}