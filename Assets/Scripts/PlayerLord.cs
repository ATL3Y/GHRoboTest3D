using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLord : MonoBehaviour
{
    public bool LostRound { get; set; }

    private int hits = 0;
    public int Hits { get { return hits; } }

    public void Init ( )
    {
        LostRound = false;
    }

    public void DisablePlayer ( )
    {

    }

    public void EnablePlayer ( )
    {
        LostRound = false;
    }

    public void Reset ( )
    {

    }

    private void OnCollisionEnter ( Collision collision )
    {

        if ( collision.gameObject.layer == 8 )
        {
            LostRound = true;
        }


        if ( collision.gameObject.GetComponent<Opponent> ( ) != null )
        {
            hits++;

            if ( hits > GameLord.Instance.Hits2WinRound - 1 )
            {
                LostRound = true;

                
                if ( Time.timeSinceLevelLoad < GameLord.Instance.PlayTime2WinGame )
                {
                    GameLord.Instance.WinState = true;
                }
            }



            if ( GameLord.Instance.Ddebug )
            {
                Debug.Log ( "Growth level: " + hits );
            }
        }
    }
}
