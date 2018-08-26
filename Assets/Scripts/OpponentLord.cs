using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentLord : MonoBehaviour
{

    private Opponent[] opponents;
    private Transform spawnPoint;

    private int index;

    public void Init ( int count, GameObject prefab, Transform trans )
    {
        opponents = new Opponent [ count ];
        spawnPoint = trans;
        index = 0;

        for ( int i = 0; i < count; i++ )
        {
            GameObject temp = Instantiate (prefab, spawnPoint.position, spawnPoint.rotation);

            if ( temp.GetComponent<Opponent> ( ) != null )
            {

                opponents [ i ] = temp.GetComponent<Opponent> ( );
                opponents [ i ].transform.SetParent ( this.transform );
                opponents [ i ].Index = i;
            }
            else
            {
                Debug.LogError ( "Opponent component missing." );
            }
        }
    }

    public void ReleaseOpponent ( )
    {
        index++;
        if ( index > opponents.Length - 1 )
        {
            index = 0;
        }
        if ( opponents [ index ] != null )
        {
            opponents [ index ].gameObject.SetActive ( true );

            if ( releaseMode )
            {
                CoHelp.Instance.DoWhen ( 3.0f, delegate { ReleaseOpponent ( ); } );
            }
        }
        else
        {
            if ( GameLord.Instance.Ddebug )
            {
                Debug.LogWarning ( "ReleaseOpponent called on null opponent." );
            }

        }


    }

    public void ResetOpponent ( int index )
    {
        opponents [ index ].transform.localPosition = spawnPoint.position;
        opponents [ index ].transform.localRotation = spawnPoint.rotation;
        opponents [ index ].gameObject.SetActive ( false );
    }

    private bool releaseMode = false;
    public void DisableOpponents ( )
    {
        releaseMode = false;
        for ( int i = 0; i < opponents.Length; i++ )
        {
            opponents [ i ].gameObject.SetActive ( false );
        }
    }

    public void EnableOpponents ( )
    {
        releaseMode = true;
        CoHelp.Instance.DoWhen ( 2.0f, delegate { ReleaseOpponent ( ); } );
    }

    public void Reset ( )
    {
        for ( int i = 0; i < opponents.Length; i++ )
        {
            opponents [ i ].transform.localPosition = spawnPoint.position;
            opponents [ i ].transform.localRotation = spawnPoint.rotation;
        }
    }
}
