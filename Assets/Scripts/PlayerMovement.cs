using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private int jumpHash;
    private int jumpLayer;
    private int punchHash;
    private int punchLayer;
    private Rigidbody rb;
    private float jumpForce = 360.0f;
    private Quaternion ogQuat;
    private float speed = 3.0f;
    private float oMove;

    public void Init ( )
    {
        anim = GetComponent<Animator> ( );
        rb = GetComponent<Rigidbody> ( );
        ogQuat = transform.rotation;
        oMove = 0.0f;


        jumpLayer = anim.GetLayerIndex ( "Jump" );
        jumpHash = Animator.StringToHash ( "Jump" );
        
        if ( GameLord.Instance.Ddebug )
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(jumpLayer);
            Debug.Log ( "Jump Layer: " + jumpLayer );
            Debug.Log ( "Jump Hash: " + jumpHash );
        }

        punchLayer = anim.GetLayerIndex ( "Punch" );
        punchHash = Animator.StringToHash ( "Punch" );
        
        if ( GameLord.Instance.Ddebug )
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo ( punchLayer );
            Debug.Log ( "Punch Layer: " + punchLayer );
            Debug.Log ( "Punch Hash: " + punchHash );
        }
    }

    public void OnBeat ( )
    {
        // HACKL3Y: Add punch to layer mask or disable punch during translation. 
        anim.SetTrigger ( punchHash );
    }

    public bool AmPunching ( )
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(punchLayer);

        if ( stateInfo.IsName ( "Punch.Punch" ) )
        // if ( stateInfo.shortNameHash == punchHash )
        {
            if ( GameLord.Instance.Ddebug )
            {
                Debug.Log ( "In punch state" );
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdatePlayerMovement ( )
    {
        bool p = AmPunching();
        // Enable double jump with quick double click on spacebar before delay times out. 
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(jumpLayer);

        if ( stateInfo.IsName ( "Jump.Jump" ) )
        // if ( stateInfo.shortNameHash == jumpHash )
        {
            if ( GameLord.Instance.Ddebug )
            {
                Debug.Log ( "In jump state" );
            }
        }
        else
        {

        }

        if ( Input.GetKeyDown ( KeyCode.Space ) )
        {
            // Frame left ground / fps / anim speed. HACKL3Y: derive this float. 
            float delay = 10.0f / 24.0f / 0.5f;
            CoHelp.Instance.DoWhen ( delay, delegate { Jump ( ); } );
            anim.SetTrigger ( jumpHash );
        }

        // Update rotation.
        float nMove = Input.GetAxis("Horizontal");
        nMove = oMove + 45.0f * Time.deltaTime * ( nMove - oMove );
        nMove = Mathf.Clamp ( nMove, -1.0f, 1.0f );

        // Rotate player about Y axis.  Right = 90 degrees.  Left = 270 degrees. 
        float degrees = 90.0f - 90.0f * nMove;
        Quaternion nQuat = Quaternion.AngleAxis ( degrees, Vector3.up );
        transform.rotation = ogQuat * nQuat;

        // Update Translation.
        Vector3 movement = new Vector3( nMove, transform.position.y, 0.0f);
        rb.velocity = movement * speed;

        // Update animation.
        float animSpeed = rb.velocity.magnitude;
        anim.SetFloat ( "Speed", animSpeed );

        // HACKL3Y: Add raycast ground and transition from jump to root anim.
        if ( transform.position.y < 0.0f )
        {
            transform.position = new Vector3 ( transform.position.x, 0.0f, transform.position.z );
        }
    }

    private void Jump ( )
    {
        rb.AddForce ( Vector3.up * jumpForce );
    }
}
