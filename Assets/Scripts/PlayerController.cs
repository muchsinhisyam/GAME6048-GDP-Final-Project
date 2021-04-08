using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
  // Start() variables
  private Rigidbody2D rb;
  private Animator anim;
  private Collider2D coll;

  // FSM
  private enum State { idle, running, jumping, falling }
  private State state = State.idle;

  // Inspector variables
  [SerializeField] private LayerMask ground;
  [SerializeField] private float speed = 5f;
  [SerializeField] private float jumpforce = 10f;
  [SerializeField] public int gems = 0;
  [SerializeField] public Text gemsText;

  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    coll = GetComponent<Collider2D>();
  }

  // Update is called once per frame
  private void Update()
  {
    PlayerMovement();
    AnimationState();
    // Sets animation based on Enumator state
    anim.SetInteger("state", (int)state);
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "Collectable")
    {
      Destroy(collision.gameObject);
      gems += 1;
      gemsText.text = gems.ToString();
    }
  }

  private void PlayerMovement()
  {
    float hDirection = Input.GetAxis("Horizontal");

    // Moving left
    if (hDirection < 0)
    {
      rb.velocity = new Vector2(-speed, rb.velocity.y);
      transform.localScale = new Vector2(-1, 1);
    }
    // Moving right
    else if (hDirection > 0)
    {
      rb.velocity = new Vector2(speed, rb.velocity.y);
      transform.localScale = new Vector2(1, 1);
    }

    // Jumping
    if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
    {
      rb.velocity = new Vector2(rb.velocity.x, jumpforce);
      state = State.jumping;
    }
  }

  private void AnimationState()
  {
    if (state == State.jumping)
    {
      if (rb.velocity.y < .1f)
      {
        state = State.falling;
      }
    }
    else if (state == State.falling)
    {
      if (coll.IsTouchingLayers(ground))
      {
        state = State.idle;
      }
    }
    else if (Mathf.Abs(rb.velocity.x) > 2f)
    {
      // Moving
      state = State.running;
    }
    else
    {
      state = State.idle;
    }
  }
}