using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public GameObject ExitUI;
    
    public Texture2D ButtonCursor;
    public Texture2D DefaultCursos;

    void Start()
    {
        Cursor.SetCursor(DefaultCursos, Vector3.zero, CursorMode.ForceSoftware);
    }

    public void OnButtonCursosEnter()
    {
        Cursor.SetCursor(ButtonCursor, Vector3.zero, CursorMode.ForceSoftware);
    }
    
    public void OnButtonCursosExit()
    {
        Cursor.SetCursor(DefaultCursos, Vector3.zero, CursorMode.ForceSoftware);
    }
    
    public void ExitGame() {
        var pieces = GameObject.FindGameObjectsWithTag("Piece");

        foreach (var piece in pieces)
        {
            piece.GetComponent<MovementScript>().PauseGame();
        }
        
        ExitUI.SetActive(true);
    }

    public void ExitGameConfirmation()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        var pieces = GameObject.FindGameObjectsWithTag("Piece");

        foreach (var piece in pieces)
        {
            piece.GetComponent<MovementScript>().ResumeGame();
        }
        
        ExitUI.SetActive(false);
    }
}
