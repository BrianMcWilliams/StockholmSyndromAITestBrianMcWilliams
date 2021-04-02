using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementStyle
{
    SideToSide,
    Diagonal,
    Random
}
public class Enemy : BoardElement
{
    public MovementStyle m_MovementStyle;

    public float m_MovementSpeed;
    public float m_TravelDistance;

    private Vector3 m_Destination;
    private Vector3 m_BaseOffset = new Vector3(0, 0.55f, 0);
    // Start is called before the first frame update
    void Start()
    {
        FindNewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if(HasReachedDestination())
        {
            FindNewDestination();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, m_Destination, Time.deltaTime * m_MovementSpeed);
            Debug.DrawLine(transform.position, m_Destination, Color.green);
        }
    }

    private void FindNewDestination()
    {
        List<Vector3> m_DestinationCandidates = new List<Vector3>();
        switch(m_MovementStyle)
        {
            case MovementStyle.SideToSide:
                {
                    GetSideToSideCandidates(m_DestinationCandidates);
                    break;
                }
            case MovementStyle.Diagonal:
                {
                    GetDiagonalCandidates(m_DestinationCandidates);
                    break;
                }
            case MovementStyle.Random:
                {
                    if(UnityEngine.Random.value > 0.5f)
                    {
                        GetSideToSideCandidates(m_DestinationCandidates);
                    }
                    else
                    {
                        GetDiagonalCandidates(m_DestinationCandidates);
                    }
                    break;
                }
        }

        while(m_DestinationCandidates.Count > 0)
        {
            int index = UnityEngine.Random.Range(0,m_DestinationCandidates.Count-1);

            Vector3 origin = transform.position + m_BaseOffset;
            Vector3 direction = m_DestinationCandidates[index] - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, m_TravelDistance + 0.5f);
            Debug.DrawLine(origin, origin + direction, Color.red);
            if(hit && hit.collider != null)
            {
                //if it's a player, we can move that way
                Player player = hit.collider.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    m_Destination = m_DestinationCandidates[index];
                    return;
                }
                else //If it's not the player, we don't want to move that way, this means monsters won't ever move on to food tiles or victory tiles
                {
                    m_DestinationCandidates.RemoveAt(index);
                }
            }
            else
            {
                m_Destination = m_DestinationCandidates[index];
                return;
            }
        }

        Console.WriteLine("Staying put because there are no valid positions");

        m_Destination = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FindNewDestination();
    }
    private void GetDiagonalCandidates(List<Vector3> m_DestinationCandidates)
    {
        m_DestinationCandidates.Add(transform.position + ((Vector3.up + Vector3.right).normalized * m_TravelDistance));
        m_DestinationCandidates.Add(transform.position + ((Vector3.down + Vector3.right).normalized * m_TravelDistance));
        m_DestinationCandidates.Add(transform.position + ((Vector3.up + Vector3.left).normalized  * m_TravelDistance));
        m_DestinationCandidates.Add(transform.position + ((Vector3.down + Vector3.left).normalized * m_TravelDistance));
    }

    private void GetSideToSideCandidates(List<Vector3> m_DestinationCandidates)
    {
        m_DestinationCandidates.Add(transform.position + (Vector3.up * m_TravelDistance));
        m_DestinationCandidates.Add(transform.position + (Vector3.down * m_TravelDistance));
        m_DestinationCandidates.Add(transform.position + (Vector3.left * m_TravelDistance));
        m_DestinationCandidates.Add(transform.position + (Vector3.right * m_TravelDistance));
    }

    private bool HasReachedDestination()
    {
        if (Vector3.Distance(transform.position, m_Destination) < 0.2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal override void HandleCollision(Player player)
    {
        player.Die();
    }
}
