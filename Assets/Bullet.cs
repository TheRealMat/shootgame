﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 50;
    public float speed = 20f;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageableScript thingHit = collision.GetComponent<DamageableScript>();
        if (thingHit != null)
        {
            thingHit.TakeDamage(damage);
        }
        Debug.Log(collision.name);
        Destroy(gameObject);
    }
}
