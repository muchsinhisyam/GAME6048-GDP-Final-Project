using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  [SerializeField] private float leftCap;
  [SerializeField] private float rightCap;
  [SerializeField] private float jumpLength = 10f;
  [SerializeField] private float jumpHeight = 15f;
  [SerializeField] private LayerMask ground;
  private Collider2D coll;
  private Rigidbody2D rb;
  private bool facingLeft = true;

  private void Start()
  {
    coll = GetComponent<Collider2D>();
    rb = GetComponent<Rigidbody2D>();
  }

  private void Update()
  {
    if (facingLeft)
    {
      // Test to see if we are beyond the leftCap
      if (transform.position.x > leftCap)
      {
        // Make sure sprite is facing right location, otherwise its face the right direction
        if (transform.localScale.x != 1)
        {
          transform.localScale = new Vector3(1, 1);
        }
        // Test to see if we are on the ground, then do jump
        if (coll.IsTouchingLayers(ground))
        {
          // Jump
          rb.velocity = new Vector2(-jumpLength, jumpHeight);
        }
      }
      else
      {
        facingLeft = false;
      }
    }

    else
    {
      if (transform.position.x < rightCap)
      {
        // Make sure sprite is facing right location, otherwise its face the right direction
        if (transform.localScale.x != -1)
        {
          transform.localScale = new Vector3(-1, 1);
        }
        // Test to see if we are on the ground, then do jump
        if (coll.IsTouchingLayers(ground))
        {
          // Jump
          rb.velocity = new Vector2(jumpLength, jumpHeight);
        }
      }
      else
      {
        facingLeft = true;
      }
    }
  }
}
