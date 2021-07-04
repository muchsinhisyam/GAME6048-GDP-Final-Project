using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  [SerializeField] private Transform[] waypoints;
  [SerializeField] private float moveSpeed = 2f;
  private Rigidbody2D rb;
  private int waypointIndex = 0;
  private void Start()
  {
    transform.position = waypoints[waypointIndex].transform.position;
  }

  private void Update()
  {
    Move();
  }

  private void Move()
  {
    if (waypointIndex <= waypoints.Length - 1)
    {
      transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, moveSpeed * Time.deltaTime);
      if (transform.position == waypoints[waypointIndex].transform.position)
      {
        waypointIndex += 1;
      }
    }
    if (waypointIndex == waypoints.Length)
    {
      waypointIndex = 0;
      // transform.localScale = new Vector2(1, 1);
    }

  }
}
