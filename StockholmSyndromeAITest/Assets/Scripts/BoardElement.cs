using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoardElementType
{
    Food,
    Enemy,
    Wall,
    Player,
    VictoryTile
}
public class BoardElement : MonoBehaviour
{
    public int m_X;
    public int m_Y;
    public BoardElementType m_ElementType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal virtual void HandleCollision(Player player)
    {
        throw new NotImplementedException();
    }
}
