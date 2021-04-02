using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    internal struct Header
    {
        public int m_Width;
        public int m_Height;
        public int m_Version; // In case we want to add features to our level generator, we can use this to manage deprecated headers
    }

    private Header m_Header;
    private byte[] m_LevelBuffer;

    [Header("File info")]
    public string m_Path;

    [Header("Monsters")]
    //enemies
    public BoardElement m_EnemyGhostBlue;
    public BoardElement m_EnemyGhostRed;
    public BoardElement m_EnemyGhostWhite;

    [Header("Food")]
    //food
    public BoardElement m_FoodTypeOrange;

    [Header("Walls")]
    //Walls
    public BoardElement m_WallTypeStone;

    [Header("Victory")]
    public BoardElement m_VictoryTile;
    [Header("Player")]
    //Player
    public BoardElement m_PlayerTypePacman;


    void Start()
    {

    }

    public GameBoard GenerateGameBoard()
    {
        GenerateLevelDataFromFile();

        GameBoard gameBoard = (GameBoard)gameObject.AddComponent(typeof(GameBoard));

        gameBoard.m_Height = m_Header.m_Height;
        gameBoard.m_Width = m_Header.m_Width;

        PopulateGameBoard(gameBoard);
        return gameBoard;
    }

    private void PopulateGameBoard(GameBoard gameBoard)
    {
        Quaternion facingCamera = Quaternion.identity;
        facingCamera.y = 180.0f;

        for (int i = 0; i < gameBoard.m_Width; ++i)
        {
            for(int j = 0; j < gameBoard.m_Height; ++j)
            {
                char element = (char)m_LevelBuffer[(i * gameBoard.m_Width) + j];

                BoardElement boardElement = IdentifyElement(element);
                if(boardElement)
                {
                    BoardElement instance = Instantiate<BoardElement>(boardElement, new Vector3(i, j, 0.0f), facingCamera);

                    if(boardElement.m_ElementType == BoardElementType.Food)
                    {
                        Food foodInstance = (Food)instance;
                        foodInstance.m_OnFoodDestroy.AddListener(gameBoard.OnFoodRemoved);
                    }

                    if (boardElement.m_ElementType == BoardElementType.Player)
                    {
                        gameBoard.m_Player = (Player)instance;
                        gameBoard.m_Player.m_Restart.AddListener(GameObject.FindObjectOfType<GameManager>().Restart);
                        gameBoard.m_CameraSwitch.AddListener(gameBoard.m_Player.SwitchCamera);
                    }

                    if (boardElement.m_ElementType == BoardElementType.VictoryTile)
                    {
                        gameBoard.m_VictoryTile = (VictoryTile)instance;
                        instance.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private BoardElement IdentifyElement(char element)
    {
        switch(element)
        {
            case 'X':
                {
                    return m_WallTypeStone;
                }
            case 'O':
                {
                    return m_EnemyGhostWhite;
                }
            case 'R':
                {
                    return m_EnemyGhostRed;
                }
            case 'B':
                {
                    return m_EnemyGhostBlue;
                }
            case 'P':
                {
                    return m_PlayerTypePacman;
                }
            case 'F':
                {
                    return m_FoodTypeOrange;
                }
            case 'V':
                {
                    return m_VictoryTile;
                }
            default:
                {
                    return null;
                }

        }
    }

    private void GenerateLevelDataFromFile()
    {
        //The first 'n' bytes of the file are the Header, this contains data to help us read the level.
        int headerSize = 42;//Marshal.SizeOf(typeof(Header));
        byte[] headerByteArray = new byte[headerSize];

        string filePath;
        if(!String.IsNullOrWhiteSpace(m_Path))
        {
            filePath = m_Path;
        }
        else
        {
            filePath = Application.dataPath + "/Level.txt";
        }
        FileStream fs = File.OpenRead(filePath);
        
        fs.Read(headerByteArray, 0, headerSize);

        string headerString = Encoding.UTF8.GetString(headerByteArray);
        m_Header = JsonUtility.FromJson<Header>(headerString);

        int levelSize = (int)(fs.Length - headerSize);

        //I would normally assert this type of thing but let's throw in case the user is trying to create his/her own levels.
        if (levelSize != (m_Header.m_Width * m_Header.m_Height))
        {
            throw new UnityException("Header level size and Width * Height are not equal, your level is likely in an incorrect format");
        }

        //I would normally assert this type of thing but let's throw in case the user is trying to create his/her own levels.
        if (m_Header.m_Width > 10 || m_Header.m_Height > 10)
        {
            throw new UnityException("Currently not supporting larger map sizes, we would need to scale everything down so it fits on screen");
        }

        // We know our levelSize is the same as our Width * Height, let's read it.
        m_LevelBuffer = new byte[levelSize];
        fs.Read(m_LevelBuffer, 0, levelSize);
        fs.Close();
    }

    void Update()
    {
        
    }
}
