using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelGenerator m_LevelGenerator;
    private GameBoard m_GameBoard;
    public Camera m_Camera;
    public GameObject m_RestartButton;

    // Start is called before the first frame update
    void Start()
    {
        InitGame();
    }

    private void InitGame()
    {
        m_GameBoard = m_LevelGenerator.GenerateGameBoard();

        float cameraX = (float)(m_GameBoard.m_Width - 1) / 2;
        float cameraY = (float)(m_GameBoard.m_Height - 1) / 2;
        
        m_Camera.gameObject.SetActive(true);
        
        m_Camera.transform.SetPositionAndRotation(new Vector3(cameraX, cameraY, -10.0f), Quaternion.identity);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        int currentHealth = m_GameBoard.m_Player.Health;

        if (currentHealth < 0)
        {
            EndGame();
        }

        int currentPoints = m_GameBoard.m_Player.Points;

        Destroy(m_GameBoard.m_VictoryTile);

        DestroyGame();

        InitGame();

        m_GameBoard.m_Player.Health = currentHealth;
        m_GameBoard.m_Player.Points = currentPoints;
    }

    private void EndGame()
    {
        m_RestartButton.SetActive(true);
    }

    public void GameReset()
    {
        Destroy(m_GameBoard.m_VictoryTile);

        DestroyGame();

        m_RestartButton.SetActive(false);

        InitGame();
    }
    public void SwitchCamera()
    {
        m_GameBoard.RequestCameraSwap();
    }
    private void DestroyGame()
    {
        BoardElement[] boardElements = GameObject.FindObjectsOfType<BoardElement>();
        foreach (BoardElement boardElement in boardElements)
        {
            Destroy(boardElement.gameObject);
        }
    }
}
