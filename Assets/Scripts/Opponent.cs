using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    private float speed = 15.0f;
    public int Index { get; set; }
    private float timerDur = 0.5f;
    private float timer;
    private Renderer rend;
    private Rigidbody rb;
    public bool Dead { get; set; }

    public void OnBeat ( )
    {
        timer = timerDur;
    }

    public void Init ( )
    {
        if ( GetComponent<Renderer> ( ) != null )
        {

            rend = GetComponent<Renderer> ( );
        }
        else
        {
            Debug.LogError ( "Renderer component missing." );
        }

        if ( GetComponent<Rigidbody> ( ) != null )
        {

            rb = GetComponent<Rigidbody> ( );
        }
        else
        {
            Debug.LogError ( "Rigidbody component missing." );
        }
    }

    public void UpdateOpponent ( )
    {
        timer -= Time.deltaTime;
        if ( timer > 0.0f )
        {
            rend.material.color = Color.yellow;
        }
        else
        {
            rend.material.color = Color.white;
        }

        //transform.localPosition -= speed * Time.deltaTime * Vector3.right;
        // rb.AddForce ( speed * Time.deltaTime * transform.forward, ForceMode.Force );

        // Derive left limit from Gamelord.
        if ( transform.position.y < -10.0f )
        {
            if ( GameLord.Instance.Ddebug )
            {
                Debug.Log ( "Resetting opponent " + Index + " due to bounds." );
            }
            GameLord.Instance.OpponentLord.ResetOpponent ( Index );
        }
    }

    public void EnableOpponent ( )
    {
        rb.AddForce ( speed * transform.forward, ForceMode.Impulse );
    }

    private void OnCollisionEnter ( Collision collision )
    {
        if ( collision.gameObject.GetComponent<PlayerLord> ( ) != null )
        {
            if ( GameLord.Instance.Player.PlayerMovement.AmPunching ( ) )
            {
                // Bad anim
                Dead = true;
                if ( GameLord.Instance.Ddebug )
                {
                    Debug.Log ( "opponent " + Index + " is dead." );
                }
                CoHelp.Instance.DoWhen ( 3.0f, delegate { GameLord.Instance.OpponentLord.ResetOpponent ( Index ); } );
            }
            else
            {
                // Yay anim
                GameLord.Instance.OpponentLord.ResetOpponent ( Index );
                if ( GameLord.Instance.Ddebug )
                {
                    Debug.Log ( "opponent " + Index + " is reset." );
                }
            }

        }

    }
}
