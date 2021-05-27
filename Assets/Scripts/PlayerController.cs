using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
  // Start() variables
  private Rigidbody2D rb;
  private Animator anim;
  private Collider2D coll;

  // FSM
  private enum State { idle, running, jumping, falling, hurt }
  private State state = State.idle;

  // Inspector variables
  [SerializeField] private LayerMask ground;
  [SerializeField] private LayerMask spikes;
  [SerializeField] private float speed = 5f;
  [SerializeField] private float jumpforce = 10f;
  [SerializeField] public int gems = 0;
  [SerializeField] public Text gemsText;
  [SerializeField] public int health = 0;
  [SerializeField] public Text healthText;
  [SerializeField] public float hurtForce = 10f;

  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    coll = GetComponent<Collider2D>();

    healthText.text = health.ToString();
  }

  // Update is called once per frame
  private void Update()
  {
    if (health <= 0)
    {
      SceneManager.LoadScene("GameOverScene");
    }
    if (state != State.hurt)
    {
      PlayerMovement();
    }
    AnimationState();
    // Sets animation based on Enumator state
    anim.SetInteger("state", (int)state);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "Collectable")
    {
      Destroy(collision.gameObject);
      gems += 1;
      gemsText.text = gems.ToString();
    }
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "Enemy")
    {
      // Rat rat = other.gameObject.GetComponent<Rat>();

      if (state == State.falling)
      {
        // rat.JumpedOn();
        Destroy(other.gameObject);
        Jump();
      }
      else
      {
        state = State.hurt;
        if (other.gameObject.transform.position.x > transform.position.x)
        {
          rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
        }
        else
        {
          rb.velocity = new Vector2(hurtForce, rb.velocity.y);
        }
        health -= 1;
        healthText.text = health.ToString();
      }
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
      Jump();
    }

    if (coll.IsTouchingLayers(spikes))
    {
      state = State.hurt;
      health -= 1;
      healthText.text = health.ToString();
    }
  }

  private void Jump()
  {
    rb.velocity = new Vector2(rb.velocity.x, jumpforce);
    state = State.jumping;
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
    else if (state == State.hurt)
    {
      if (Mathf.Abs(rb.velocity.x) < .1f)
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
