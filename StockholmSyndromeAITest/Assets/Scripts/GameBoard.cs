using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameBoard : MonoBehaviour
{
    public int m_Width;
    public int m_Height;

    public List<BoardElement> m_FoodList = new List<BoardElement>();
    public BoardElement m_VictoryTile;
    public Player m_Player;
    public UnityEvent m_CameraSwitch = new UnityEngine.Events.UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFoodRemoved()
    {
        if (GameObject.FindObjectsOfType<Food>().Length == 0)
            m_VictoryTile.gameObject.SetActive(true);
    }

    public void RequestCameraSwap()
    {
        m_CameraSwitch.Invoke();
    }
}
