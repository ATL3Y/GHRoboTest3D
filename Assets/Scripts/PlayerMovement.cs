using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private int jumpHash = Animator.StringToHash("Jump");
    // private int runStateHash = Animator.StringToHash("Base Layer.Run");
    private Rigidbody rb;
    private float jumpForce = 290.0f;


    // Use this for initialization
    private void Start ( )
    {
        anim = GetComponent<Animator> ( );
        rb = GetComponent<Rigidbody> ( );
    }

    // Update is called once per frame
    private void Update ( )
    {
        float move = Input.GetAxis("Vertical");
        anim.SetFloat ( "Speed", move );

        float turnSpeed = Input.GetAxis("Horizontal");
        anim.SetFloat ( "TurnSpeed", turnSpeed );

        // AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if ( Input.GetKeyDown ( KeyCode.Space ) ) //&& stateInfo.nameHash == runStateHash )
        {
            float delay = 10.0f / 24.0f / 0.5f;
            CoHelp.Instance.DoWhen ( delay, delegate { Jump ( ); } );
            anim.SetTrigger ( jumpHash );
        }
    }

    private void Jump ( )
    {
        rb.AddForce ( Vector3.up * jumpForce );
    }
}
