using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using TMPro;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameLogicMulti : NetworkBehaviour
{
    // Public references of objects in the scene, we will need those to interact with them:

    GameObject boardPieceHolders;
    public GameObject boardPieceHolder;
 
	
    public GameObject boardPiece;
    public GameObject boardPiecey;
    public  Material machineMaterial;
    public  Material playerMaterial;
    public static bool controlEnabled = true;
    public GameObject mainMenuBody;
    public GameObject board; 
    public Camera mainCamera;

    // We will be using this audiosource to play sound effects:
    public AudioSource audioSourceEffects;
    public AudioClip moveEffect; // This is a reference to the "movement" sound effect.
    public AudioClip newGameEffect;
    public AudioClip errorEffect;
    public AudioClip gameOverEffect;

    public CameraBehaviorMulti cameraBehavior;
    public TextMeshProUGUI text;

    public bool isPlaying = false;
    private int difficult = 6;

    public PostProcessingProfile highPP;

    // We need to store the local positions of where the pieces should go:
    private static float[] rows = new float[]{
        -0.1698f,
        -0.1148f,
        -0.057f,
        0.0013f,
        0.0587f,
        0.117f
    };
    private static float[] columns = new float[]{
        -0.0888f,
        -0.0289f,
        0.0298f,
        0.0867f,
        0.1442f,
        0.2017f,
        0.2586f
    };

    // Below are the two instances of the IA: the current board and the Minimax algorithm:
  
    public static Board currentGameBoard;
    private Minimax<Board> gameAI;
    private Connect4GameRules gameRules;
    private bool autoPlay = false;

    
	public static bool todo = false ; 

    void Start()
    {
		
        currentGameBoard = new Board();
        gameRules = new Connect4GameRules();
        gameAI = new Minimax<Board>(gameRules);
        this.boardPieceHolder = GameObject.Find("piecesHolder");

       
        StartGame(4);
        //	Cmdrenders ();
    }


	void Update(){


	
        
		if (isLocalPlayer)
        {
            if (todo)
            {
                if (isServer)
                {
                    CmdRegisterPlayerMovement(HitboxBehaviorMulti.column);
                    Debug.Log("From Host");
                    todo = false;
                }
                else if (!isServer)
                {
                    CmdRegisterPlayerMovementClient(HitboxBehaviorMulti.column);
                    Debug.Log("From Cleint");
                    todo = false;
                }
                
            }
        }
		

	}



	public override void OnStartClient() {
		base.OnStartClient();

    }

    
    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
      
        DestroyObject(GameObject.Find("CustomNetworkManager"));
        DestroyObject(GameObject.Find("GameLogicHolderMulti(Clone)"));
        Application.LoadLevel("GameWorld");

    }


    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            if (isServer)
            {
                setPricHold();
                Debug.Log("i am Host");
            }
            else
            {
                setPricHold();
                Debug.Log("i am client");
            }

        }

    }

    /**
     * Clears the board.
     */
    public void ClearBoard()
    {
        foreach (Transform piece in boardPieceHolder.transform)
            GameObject.Destroy(piece.gameObject);
    }



	public void setPricHold (){
        // 	this.boardPieceHolder = GameObject.Find ("piecesHolder");
        //  this.board = GameObject.Find("Boards");
        //   boardPieceHolders = Instantiate(boardPieceHolder);
        //  boardPieceHolders.transform.parent = board.transform;
        //  boardPieceHolders.transform.localPosition = new Vector3(11.233f, 24.98f, -47.73f);
      //  this.mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
     //   this.cameraBehavior = GameObject.Find("Main Camera").GetComponent<CameraBehaviorMulti>();
        this.audioSourceEffects = GameObject.Find ("Audio Source").GetComponent<AudioSource> ();
     //   this.mainMenuBody = GameObject.Find("MainMenu");
	}



    /**
     * Setup the game, board, camera and logic.
     */
    public void StartGame(int depth)
    {
        currentGameBoard = new Board();
        ClearBoard();
        text.text = "";
        /*
        if (isServer)
        {
            controlEnabled = true;
        }*/
       
        this.difficult = depth;
        this.audioSourceEffects.PlayOneShot(newGameEffect, 1f);
        Debug.Log("start game");
        ResumeGame();
       
    }

    
	public void ResumeGame()
	{
		cameraBehavior.enableControl();
		cameraBehavior.moveCameraTo(cameraBehavior.GamePosition);
		isPlaying = true;
	}
   
    /**
     * Returns to the main menu.
     */
    public void MainMenu()
    {
        cameraBehavior.disableControl();
        cameraBehavior.moveCameraTo(cameraBehavior.MenuPosition);
        isPlaying = false;
    }

   

    
   

    /**
     * Returns to the current game.
     */


    /**
     * This function registers the human player movement.
     */



    [Command]
    public void CmdRegisterPlayerMovement(int column)
    {
        // If the controls are enabled, simple register the player movement on the board and start the
        if (!controlEnabled) { audioSourceEffects.PlayOneShot(errorEffect, 1); return; }
		
			
        if (currentGameBoard.registerPlayer1Movement(column))
            {
            // We now will render the board again and start the Minimax execution:

            CmdrenderBoard(currentGameBoard, false);
           
            int winner = 0;

            // Checking if the game is over:
            if (gameRules.IsGameOver(currentGameBoard, ref winner))
                StartCoroutine(executeWonScene(winner));
            else
                // The game is not over, let the AI play:
                //  StartCoroutine(executeMachinePlay());
                controlEnabled = false;
				Debug.Log("AI");
        }
        // This is a illegal move, play a error effect:
        else audioSourceEffects.PlayOneShot(errorEffect, 1);
		 }





    [Command]
    public void CmdRegisterPlayerMovementClient(int column)
    {
        // If the controls are enabled, simple register the player movement on the board and start the
        if (controlEnabled) { audioSourceEffects.PlayOneShot(errorEffect, 1); return; }


        if (currentGameBoard.registerPlayer2Movement(column))
        {
            // We now will render the board again and start the Minimax execution:

            CmdrenderBoard(currentGameBoard, false);

            int winner = 0;

            // Checking if the game is over:
            if (gameRules.IsGameOver(currentGameBoard, ref winner))
                StartCoroutine(executeWonScene(winner));
            else
                // The game is not over, let the AI play:
                //  StartCoroutine(executeMachinePlay());
                controlEnabled = true;
                Debug.Log("AI");
        }
        // This is a illegal move, play a error effect:
        else audioSourceEffects.PlayOneShot(errorEffect, 1);
    }


    /*
     * Renders the pieces of the board, should be called everytime the board has changed.
     */


    [Command]
    public void CmdrenderBoard(Board board, bool fastUpdate)
    {
        //Clear the board when we are doing a full update:
        if (!fastUpdate) ClearBoard();

        // We always call this function when a new move is made, so let's play a sound effect:
        audioSourceEffects.PlayOneShot(moveEffect, 1);

        
        for (int x = 0; x < board.Get.Length; x++)
            for (int y = 0; y < board.Get[x].Length; y++)
                if (board.Get[x][y] != 0)
                    // For each position in the board, if it's not empty and we are doing a full update:
                    if (!fastUpdate || (currentGameBoard.Get[x][y] != board.Get[x][y]))
                    {
                        if (board.Get[x][y] == 1)
                        {
                            // Add this piece to the board:
                            GameObject piece = Instantiate(boardPiece);

                            piece.transform.parent = boardPieceHolder.transform;
                            piece.transform.localPosition = new Vector3(columns[y], rows[x], 0.1031f);

                            //  piece.GetComponent<Renderer>().material = board.Get[x][y] == -1 ? machineMaterial : playerMaterial;
                            NetworkServer.Spawn(piece);
                        }
                        else if (board.Get[x][y] == -1)
                        {
                            // Add this piece to the board:
                            GameObject piece = Instantiate(boardPiecey);

                            piece.transform.parent = boardPieceHolder.transform;
                            piece.transform.localPosition = new Vector3(columns[y], rows[x], 0.1031f);

                            //  piece.GetComponent<Renderer>().material = board.Get[x][y] == -1 ? machineMaterial : playerMaterial;
                            NetworkServer.Spawn(piece);
                        }
                       
                    }

        // Usefull when we are not doing a full update to sync the current state of the game:
       currentGameBoard = board;
    }


    



    /**
     * Waits 2 seconds and proceeds to the game over screen.
     */
    IEnumerator executeWonScene(int winner)
    {
        audioSourceEffects.PlayOneShot(gameOverEffect, 1);
        controlEnabled = false;
       

        // We will wait a little so the player have time to see the board.
       
        
            bool isWorking = true;
            new Thread(() =>
            {
                
                Thread.Sleep(2000);
                isWorking = false;
            }).Start();


            while (isWorking)
                yield return null;
            // Now we go to the game over screen:
            GameOver(winner);
      
    }



    /**
    * Deals with the necessary chances to the game's state to 
    * display the game over screen.
    */
    public void GameOver(int winner)
    {
        NetworkManager.Shutdown();
        CustomNetworkManager.Shutdown();
        controlEnabled = false;
        DestroyObject(GameObject.Find("CustomNetworkManager"));
        DestroyObject(GameObject.Find("GameLogicHolderMulti(Clone)"));
        Application.LoadLevel("GameWorld");


        //   cameraBehavior.disableControl();
        //  cameraBehavior.moveCameraTo(cameraBehavior.MenuPosition);
        //  mainMenuBody.SetActive(true);

        // Display who won:
        if (winner == 0)
            text.text = "The game ended with a tie!";
        else
        {
            if (autoPlay)
                text.text = (winner == -1 ? "Yellow" : "Red") + " player won!";
            else
            {
                if (winner == 1) text.text = "YOU WON!";
                else if (winner == -1) text.text = "YOU LOSE.";
            }
        }
        isPlaying = false;
        autoPlay = false;

        //  NetworkManager.singleton.StopHost();
        //  NetworkManager.singleton.StopServer();

    }

    /**
     * Disable all post processing effects if it enabled or enables them if currently disabled.
     */
    public void SwitchPPQuality()
    {
        PostProcessingBehaviour cPP = mainCamera.GetComponent<PostProcessingBehaviour>();
        cPP.enabled = !cPP.enabled;
    }
}
