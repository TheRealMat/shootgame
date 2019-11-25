﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private float walkSpeed = 70.0F;
    private float rotationSpeed = 15.0F;
    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            rb2D.AddForce(transform.up * walkSpeed);
        }
        if (Input.GetKey("s"))
        {
            rb2D.AddForce(transform.up * -walkSpeed);
        }
        if (Input.GetKey("a"))
        {
            rb2D.AddForce(transform.right * -walkSpeed);
        }
        if (Input.GetKey("d"))
        {
            rb2D.AddForce(transform.right * walkSpeed);
        }
    }
}
