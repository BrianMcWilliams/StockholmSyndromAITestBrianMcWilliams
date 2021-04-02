using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Player : BoardElement
{
    Vector3 m_Direction;
    public float m_Speed;
    public GameObject m_PlayerCamera;
    private GameObject m_MainCamera;
    private int m_Points;
    public UnityEvent m_Restart;

    public int Points
    {
        get { return this.m_Points; }
        set
        {
            this.m_Points = value;
            GameObject pointsLabelObject = GameObject.FindGameObjectWithTag("PointsLabel");
            pointsLabelObject.GetComponent<TextMeshProUGUI>().text = this.m_Points.ToString();
        }
    }
    private int m_Health =3;
    public int Health { 
        get { return m_Health; }
        set {
            this.m_Health = value;

            UpdateHealthLabel();
        } 
    }

    private void UpdateHealthLabel()
    {
        GameObject healthLabelObject = GameObject.FindGameObjectWithTag("LivesLabel");
        healthLabelObject.GetComponent<TextMeshProUGUI>().text = this.m_Health.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthLabel();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector3 newPosition = Vector3.MoveTowards(gameObject.transform.position, gameObject.transform.position + m_Direction, m_Speed * Time.deltaTime);
        gameObject.transform.position = newPosition;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            m_Direction = new Vector3(0, 1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            m_Direction = new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            m_Direction = new Vector3(0, -1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            m_Direction = new Vector3(1, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BoardElement boardElement = collision.gameObject.GetComponent<BoardElement>();

        if (!boardElement)
            throw new UnityException("No boardElement on this collision, what did you collide with!?");

        boardElement.HandleCollision(this);

    }

    internal void IncreasePoints(int m_PointsValue)
    {
        Points += m_PointsValue;
    }

    internal void Die()
    {
        --Health;

        m_Restart.Invoke();
    }

    internal void NextLevel()
    {
        m_Restart.Invoke();
    }
        
    internal void SwitchCamera()
    {
        if(!m_PlayerCamera.gameObject.activeSelf)
        {
            m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            m_MainCamera.SetActive(false);
            m_PlayerCamera.gameObject.SetActive(true);
        }
        else
        {
            m_PlayerCamera.gameObject.SetActive(false);
            m_MainCamera.SetActive(true);
            m_MainCamera = null;
        }
    }
}
