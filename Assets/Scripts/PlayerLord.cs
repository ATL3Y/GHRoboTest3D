using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLord : MonoBehaviour
{
    private bool lostRound;
    public bool LostRound { get { return lostRound; } }

    private int hits = 0;
    public int Hits { get { return hits; } }

    public void Init ( )
    {
        lostRound = false;
    }

    public void DisablePlayer ( )
    {

    }

    public void EnablePlayer ( )
    {
        lostRound = false;
    }

    public void Reset ( )
    {

    }

    private void OnCollisionEnter ( Collision collision )
    {
        if ( collision.gameObject.GetComponent<Opponent> ( ) != null )
        {
            hits++;

            if ( hits > GameLord.Instance.Hits2WinRound - 1 )
            {
                lostRound = true;
                if ( Time.timeSinceLevelLoad < GameLord.Instance.PlayTime2WinGame )
                {
                    GameLord.Instance.WinState = true;
                }
            }



            if ( GameLord.Instance.Ddebug )
            {
                Debug.Log ( "Hits: " + hits + ", Win State: " + GameLord.Instance.WinState );
            }
        }


    }
}
