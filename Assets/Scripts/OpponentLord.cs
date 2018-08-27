using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentLord : MonoBehaviour
{

    private Opponent[] opponents;

    private int index;

    public void Init ( int count, GameObject prefab )
    {
        opponents = new Opponent [ count ];
        index = 0;

        for ( int i = 0; i < count; i++ )
        {
            GameObject temp = Instantiate (prefab, Vector3.zero, Quaternion.identity);

            if ( temp.GetComponent<Opponent> ( ) != null )
            {

                opponents [ i ] = temp.GetComponent<Opponent> ( );
                opponents [ i ].transform.SetParent ( this.transform );
                opponents [ i ].Index = i;
                opponents [ i ].Dead = false;
                opponents [ i ].Init ( );
            }
            else
            {
                Debug.LogError ( "Opponent component missing." );
            }
        }
    }

    public void OnBeat ( )
    { 
    
        if ( GameLord.Instance.Ddebug )
        {
            Debug.Log ( "OpponentLord.OnBeat called." );
        }

        ReleaseOpponent ( );
        for ( int i = 0; i < opponents.Length; i++ )
        {
            opponents [ i ].OnBeat ( );
        }
    }

    public void ReleaseOpponent ( )
    {
        if ( GameLord.Instance.Ddebug )
        {
            Debug.Log ( "ReleaseOpponent called." );
        }

        if ( GameLord.Instance.GameState != GameLord.GameStates.Playing )
        {
            if ( GameLord.Instance.Ddebug )
            {
                Debug.LogWarning ( "Trying to release in not playing." );
            }
            
            return;
        }

        index++;
        if ( index > opponents.Length - 1 )
        {
            index = 0;
        }

        if ( opponents [ index ] != null )
        {
            if ( !opponents [ index ].Dead )
            {
                if ( GameLord.Instance.Ddebug )
                {
                    Debug.Log ( "Releasing opponent." );
                }

                opponents [ index ].gameObject.SetActive ( true );
                opponents [ index ].transform.position = new Vector3 ( -10.0f, 6.0f * GameLord.Instance.Player.transform.localScale.y, 0.0f );
                opponents [ index ].transform.rotation *= Quaternion.LookRotation ( GameLord.Instance.Player.transform.position - opponents [ index ].transform.position );
                opponents [ index ].EnableOpponent ( );
            }
            else
            {
                if ( GameLord.Instance.Ddebug )
                {
                    Debug.Log ( "Trying to release but opponent is dead." );
                }
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

    public void UpdateOpponentLord ( )
    {
        
        for ( int i = 0; i < opponents.Length; i++ )
        {
            if ( opponents [ i ].gameObject.activeInHierarchy )
            {
                opponents [ i ].UpdateOpponent ( );
            }
            
        }
    }

    public void ResetOpponent ( int index )
    {
        opponents [ index ].gameObject.SetActive ( false );
        opponents [ index ].transform.position = Vector3.zero;
        opponents [ index ].transform.rotation = Quaternion.identity;
    }

    public void DisableOpponents ( )
    {
        for ( int i = 0; i < opponents.Length; i++ )
        {
            opponents [ i ].gameObject.SetActive ( false );
            opponents [ index ].transform.position = Vector3.zero;
            opponents [ index ].transform.rotation = Quaternion.identity;
        }
    }

    public void EnableOpponents ( )
    {
        // CoHelp.Instance.DoWhen ( 2.0f, delegate { ReleaseOpponent ( ); } );
    }

    public void Reset ( )
    {

    }
}
