using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLord : MonoBehaviour
{
    private int hits = 0;
    public int Hits { get { return hits; } }
    private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement { get { return playerMovement; } }

    private bool playMode;

    public void Init ( )
    {
        playMode = false;

        if ( GetComponent<PlayerMovement> ( ) != null )
        {
            playerMovement = GetComponent<PlayerMovement> ( );
        }
        else
        {
            Debug.LogError ( "PlayerMovement component not found." );
        }


        playerMovement.Init ( );
    }

    public void OnBeat ( )
    {
        if ( playMode )
        {
            playerMovement.OnBeat ( );
        }

    }


    public void DisablePlayer ( )
    {
        playMode = false;
    }

    public void EnablePlayer ( )
    {
        playMode = true;
    }

    public void Reset ( )
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.LookRotation ( Vector3.right );
    }

    public void UpdatePlayerLord ( )
    {
        playerMovement.UpdatePlayerMovement ( );

    }

    private void OnCollisionEnter ( Collision collision )
    {
        // If I hit a limit, I lose.
        if ( collision.gameObject.layer == 8 )
        {
            GameLord.Instance.GameState = GameLord.GameStates.Lost;
        }


        if ( collision.gameObject.GetComponent<Opponent> ( ) != null )
        {
            // If I punch an opponent, I lose a growth point. 
            if ( playerMovement.AmPunching ( ) )
            {
                hits--;
            }
            // If I get hit by an opponent, I gain a growth point.
            else
            {
                hits++;
            }

            if ( GameLord.Instance.Ddebug )
            {
                Debug.Log ( "Hits: " + hits );
            }

            if ( hits < 0 )
            {
                GameLord.Instance.GameState = GameLord.GameStates.Lost;
            }
        }
    }
}
