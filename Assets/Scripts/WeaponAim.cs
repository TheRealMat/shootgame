using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponAim : MonoBehaviour
{
    private SpriteRenderer playerSprite;
    private SpriteRenderer weaponSprite;
    private bool facingRight = false;
    private bool directionChanged = false;
    Transform parent;
    public Transform firePoint;
    public int damage = 50;
    public LineRenderer lineRenderer;
    public double inaccuracy = 5;
    private void Start()
    {
        parent = gameObject.transform.parent;
        weaponSprite = GetComponent<SpriteRenderer>();
        playerSprite = parent.GetComponent<SpriteRenderer>();
        lineRenderer.useWorldSpace = true;

    }
    void Update()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // gets angle between mouse and player
        var angle = Mathf.Atan2(point.y - parent.position.y, point.x - parent.position.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.position = Vector2.MoveTowards(new Vector2(parent.position.x, parent.position.y), new Vector2(point.x, point.y), 0.3F);



        // this is bad. rewrite
        if (point.x > parent.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
            transform.Rotate(180f, 0f, 0f);
        }
        if (facingRight != directionChanged)
        {
            directionChanged = facingRight;
            FlipSprite();
        }


        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shoot());
        }
    }
    private void FlipSprite()
    {
        parent.Rotate(0f, 180f, 0f);
    }
    public double GetRandomNumberInRange(double minNumber, double maxNumber)
    {
        return (new System.Random().NextDouble() * (maxNumber - minNumber) + minNumber);
    }

    IEnumerator Shoot()
    {
        Quaternion previousAngle = firePoint.rotation;
        firePoint.Rotate(0, 0, (float) GetRandomNumberInRange(-inaccuracy, inaccuracy), Space.World);
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        if (hitInfo)
        {
            Debug.Log("firepoint = " + firePoint.position);
            Debug.Log("hitinfo.point = " + hitInfo.point);
            DamageableScript hitThing = hitInfo.transform.GetComponent<DamageableScript>();
            if (hitThing != null)
            {

                hitThing.TakeDamage(damage);
            }
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.localPosition);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }
        firePoint.rotation = previousAngle;
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.02f);
        lineRenderer.enabled = false;

    }
}
