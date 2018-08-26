using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    private float speed = 3.5f;
    public int Index { get; set; }

    private void Update ( )
    {
        transform.localPosition -= speed * Time.deltaTime * Vector3.right;
    }

    private void OnCollisionEnter ( Collision collision )
    {
        GameLord.Instance.OpponentLord.ResetOpponent ( Index );
    }
}
