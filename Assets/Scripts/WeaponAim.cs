using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// entire script should be separated into weapon things and movement things
public class WeaponAim : MonoBehaviour
{

    Transform parent;
    public Transform firePoint;
    public int damage = 50;
    public LineRenderer lineRenderer;
    public double inaccuracy = 5;
    public bool isAuto = false;
    public double firingSpeed;
    public SpriteRenderer muzzleFlash;
    public GameObject weapon;
    private Animator animator;
    // ideally speed of the recoil animation should be tied to the firing speed somehow
    private AnimationClip recoilAnim;
    public bool facingRight = false;
    private bool currentDirection = false;
    private bool previousDirection;

    private float lastFired;
    private void Start()
    {
        animator = weapon.GetComponent<Animator>();
        parent = gameObject.transform.parent;
        lineRenderer.useWorldSpace = true;
    }
    void Update()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // gets angle between mouse and player
        var angle = Mathf.Atan2(point.y - parent.position.y, point.x - parent.position.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // needs some kind of offset to make it feel more natural
        transform.position = Vector2.MoveTowards(new Vector2(parent.position.x, parent.position.y), new Vector2(point.x, point.y), 0.3F);
        if (Input.GetMouseButtonDown(0) && isAuto == false)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetMouseButton(0) && isAuto == true)
        {
            StartCoroutine(Shoot());
        }
    }




    public double GetRandomNumberInRange(double minNumber, double maxNumber)
    {
        return (new System.Random().NextDouble() * (maxNumber - minNumber) + minNumber);
    }

    IEnumerator Shoot()
    {
        if (Time.time - lastFired > 1 / firingSpeed)
        {
            animator.Play("RecoilAnim");
            lastFired = Time.time;

            Quaternion previousAngle = firePoint.rotation;
            firePoint.Rotate(0, 0, (float)GetRandomNumberInRange(-inaccuracy, inaccuracy), Space.World);
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
            muzzleFlash.enabled = true;
            lineRenderer.enabled = true;
            yield return new WaitForSeconds(0.02f);
            lineRenderer.enabled = false;
            muzzleFlash.enabled = false;
        }


    }
}
