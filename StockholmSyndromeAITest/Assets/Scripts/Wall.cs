using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BoardElement
{
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
        // If I were a teleporter I could set player position here;
    }
}
