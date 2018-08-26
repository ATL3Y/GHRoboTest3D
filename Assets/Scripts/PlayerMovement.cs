using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private int jumpHash = Animator.StringToHash("Jump");
    int runStateHash = Animator.StringToHash("Base Layer.Run");


    // Use this for initialization
    private void Start ( )
    {
        anim = GetComponent<Animator> ( );
    }

    // Update is called once per frame
    private void Update ( )
    {
        float move = Input.GetAxis("Horizontal");
        anim.SetFloat ( "Speed", move );

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if ( Input.GetKeyDown ( KeyCode.Space ) && stateInfo.nameHash == runStateHash )
        {
            anim.SetTrigger ( jumpHash );
        }
    }
}
