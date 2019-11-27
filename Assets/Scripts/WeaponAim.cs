using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    private SpriteRenderer playerSprite;
    private SpriteRenderer weaponSprite;
    private bool facingRight = false;
    private bool directionChanged = false;
    Transform parent;
    private void Start()
    {
        parent = gameObject.transform.parent;
        weaponSprite = GetComponent<SpriteRenderer>();
        playerSprite = parent.GetComponent<SpriteRenderer>();

    }
    void Update()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // gets angle between mouse and player
        var angle = Mathf.Atan2(point.y - parent.position.y, point.x - parent.position.x) * Mathf.Rad2Deg;

        //var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.position = Vector2.MoveTowards(new Vector2(parent.position.x, parent.position.y), new Vector2(point.x, point.y), 0.3F);





        if (point.x > parent.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }
        if (facingRight != directionChanged)
        {
            directionChanged = facingRight;
            FlipSprite();
        }
    }
    private void FlipSprite()
    {
        weaponSprite.flipY = !weaponSprite.flipY;

        playerSprite.flipX = !playerSprite.flipX;
        if (playerSprite.flipX == true)
        {
            transform.position = new Vector2(transform.position.x + 0, transform.position.y);
        }
    }
}
