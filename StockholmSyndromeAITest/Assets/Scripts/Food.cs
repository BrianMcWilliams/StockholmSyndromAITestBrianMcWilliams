using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Food : BoardElement
{
    public UnityEvent m_OnFoodDestroy;
    public int m_PointsValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        m_OnFoodDestroy.Invoke();
    }

    internal override void HandleCollision(Player player)
    {
        player.IncreasePoints(m_PointsValue);
        
        Destroy(this.gameObject);
    }
}
