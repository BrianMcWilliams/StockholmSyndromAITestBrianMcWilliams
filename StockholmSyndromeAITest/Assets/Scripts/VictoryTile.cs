using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTile : BoardElement
{
    public int m_Points;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal override void HandleCollision(Player player)
    {
        player.Points += m_Points;

        player.NextLevel();
    }
}
