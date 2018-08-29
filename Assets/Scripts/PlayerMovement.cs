using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 36.0f;

    public float hAccel = 20.0f;
    public float maxHSpeed = 12.0f;
    public float hDeccel = 20.0f;

    private Animator anim;
    private int jumpHash;
    private int jumpLayer;
    private int punchHash;
    private int punchLayer;
    private Rigidbody rb;

    private Quaternion ogQuat;
    private float speed = 3.0f;

    private bool isOnGround;

    private Vector3 velocity = Vector3.zero;

    private float facing = 0.0f;
    private float animatedFacing = 0.0f;

    [SerializeField]
    private GameObject hitBox;

    public void Init ( )
    {
        anim = GetComponent<Animator> ( );
        rb = GetComponent<Rigidbody> ( );
        ogQuat = transform.rotation;


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
        if ( Input.GetKeyDown ( KeyCode.C ) )
        {
            anim.SetTrigger ( punchHash );
        }
        if ( AmPunching ( ) )
        {
            hitBox.SetActive ( true );
        }else
        {
            hitBox.SetActive ( false );
        }

        if ( Input.GetKeyDown ( KeyCode.X ) )
        {
            velocity.y = jumpForce;
            isOnGround = false;
            anim.SetTrigger ( jumpHash );
        }
        // Simulate momentum.
        float hInput = Input.GetAxisRaw("Horizontal");

        Vector3 accel = new Vector3(0.0f, 0.0f, 0.0f);
        accel.x = hInput * hAccel;
        accel.y = Physics.gravity.y;
        velocity.x = Mathf.Lerp ( velocity.x, 0.0f, hDeccel * Time.deltaTime );
        velocity += accel * Time.deltaTime;
        velocity.x = Mathf.Clamp ( velocity.x, -maxHSpeed, maxHSpeed );

        if ( hInput > 0.0f )
        {
            facing = 1.0f;
        }
        else if ( hInput < 0.0f )
        {
            facing = -1.0f;
        }

        transform.position += velocity * Time.deltaTime;

        // Update animation.
        float animSpeed = Mathf.Abs(velocity.x);
        anim.SetFloat ( "Speed", animSpeed );

        // HACKL3Y: Add raycast ground and transition from jump to root anim.
        if ( transform.position.y <= 0.0f )
        {
            velocity.y = 0.0f;
            isOnGround = true;
            transform.position = new Vector3 ( transform.position.x, 0.0f, transform.position.z );
        }
        else
        {
            isOnGround = false;
        }

        // Rotate player about Y axis.  Right = 90 degrees.  Left = 270 degrees. 
        animatedFacing = Mathf.MoveTowards ( animatedFacing, facing, Time.deltaTime * 10.0f );
        float degrees = 90.0f - 90.0f * animatedFacing;
        Quaternion nQuat = Quaternion.AngleAxis ( degrees, Vector3.up );
        transform.rotation = ogQuat * nQuat;

    }
}
