using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private int jumpHash = Animator.StringToHash("Jump");
    private int punchHash = Animator.StringToHash("Punch");
    private Rigidbody rb;
    private float jumpForce = 290.0f;
    private Quaternion oQuat;
    private float speed = 3.0f;

    public void Init ( )
    {
        anim = GetComponent<Animator> ( );
        rb = GetComponent<Rigidbody> ( );
        oQuat = transform.rotation;
    }

    public void OnBeat ( )
    {
        // HACKL3Y: Add punch to layer mask or disable punch during translation. 
        // anim.SetTrigger ( punchHash );
    }

    public bool AmPunching ( )
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if( stateInfo.fullPathHash == punchHash )
        {
            return true;
        }else
        {
            return false;
        }
    }

    public void UpdatePlayerMovement ( )
    {
        // Update animation.
        float move = Mathf.Clamp01( Input.GetAxis("Vertical") );
        anim.SetFloat ( "Speed", move );

        // Limit jump to a state.  HACKL3Y: This isn't working.
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if ( Input.GetKeyDown ( KeyCode.Space ) && stateInfo.fullPathHash != jumpHash )
        {
            // Frame left ground / fps / anim speed. HACKL3Y: derive this float. 
            float delay = 10.0f / 24.0f / 0.5f;
            CoHelp.Instance.DoWhen ( delay, delegate { Jump ( ); } );
            anim.SetTrigger ( jumpHash );
        }

        // HACKL3Y: Add raycast ground and transition from jump to root anim.

        // Update rotation and translation.
        float turnSpeed = Input.GetAxis("Horizontal");

        // Rotate player about Y axis.  Right = 90 degrees.  Left = 270 degrees. 
        float degrees = 90.0f * -turnSpeed + 90.0f;
        Quaternion nQuat = Quaternion.AngleAxis ( degrees, Vector3.up );
        transform.rotation = oQuat * nQuat;

        Vector3 dir = Vector3.Dot(Vector3.right, transform.forward) * Vector3.right;
        dir.Normalize ( );
        Vector3 offset = speed * move * Time.deltaTime * dir;
        transform.position += offset;
    }

    private void Jump ( )
    {
        rb.AddForce ( Vector3.up * jumpForce );
    }
}
