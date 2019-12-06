using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponAim : MonoBehaviour
{

    public Transform parent;

    public GameObject weapon;





    private void Start()
    {

        parent = gameObject.transform.parent;

    }
    void Update()
    {








        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // gets angle between mouse and player
        var angle = Mathf.Atan2(point.y - parent.position.y, point.x - parent.position.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // needs some kind of offset to make it feel more natural
        transform.position = Vector2.MoveTowards(new Vector2(parent.position.x, parent.position.y), new Vector2(point.x, point.y), 0.3F);

        // terrible implementation, fix asap
        if (point.x < parent.position.x)
        {
            parent.GetComponent<SpriteRenderer>().flipX = true;
            transform.Rotate(180, 0, 0);
        }
        else if (point.x > parent.position.x)
        {
            parent.GetComponent<SpriteRenderer>().flipX = false;
            transform.Rotate(0, 0, 0);
        }
        if (weapon.transform.parent != null)
        {
            weapon.GetComponent<Gun>().Input();
        }

    }
}
