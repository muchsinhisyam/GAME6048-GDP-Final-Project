using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-5, 0);
        }
    }
}
