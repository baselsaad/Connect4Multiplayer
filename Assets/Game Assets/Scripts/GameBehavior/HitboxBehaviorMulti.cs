using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class HitboxBehaviorMulti  : NetworkBehaviour
{
    private Renderer render;
	public  GameLogicMulti gameLogic;
    public Texture2D cursorGrabTexture;
  //  private bool setGL = false;
  //  public static bool onStartCon = false;


    public static int column ;


    void Start()
    {
        render = GetComponent<Renderer>();

       
       
    }
     void Update()
    {
	
/*
        if (onStartCon)
        {
            if (!setGL)
            {
                setGamelogic();
            }
        }
       
        */
    }




    public  void setGamelogic (){

		this.gameLogic = GameObject.Find ("GameLogicHolderMulti(Clone)").GetComponent<GameLogicMulti> ();
		//Debug.Log ("set gamelogic ");
       // setGL = true;
     
    }



    void OnMouseEnter()
    {
        render.enabled = true;
        Cursor.SetCursor(cursorGrabTexture, Vector2.zero, CursorMode.Auto);
    }
    void OnMouseExit()
    {
        render.enabled = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

	

    void OnMouseUpAsButton()
    {
		setGamelogic ();
        //column = int.Parse(this.tag.Replace("column_", ""));
        //ganeLogic.RegisterPlayerBewegung(column);


        
        GameLogicMulti.todo = true;
        column = int.Parse(this.tag.Replace("column_", ""));

        /*
       if (isLocalPlayer)
        {
            int column = int.Parse(this.tag.Replace("column_", ""));
            gameLogic.CmdRegisterPlayerMovement(column);
        }   
        */



    }
}
