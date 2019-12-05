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

    private bool isReloading = false;

    public int magAmount;
    public int MaxBulletsInMag;
    public int currentBulletsInMag;

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


        if (isReloading == false)
        {
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

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
            if (Input.GetKeyDown("r"))
            {
                StartCoroutine(Reload());
            }
        }

    }




    public double GetRandomNumberInRange(double minNumber, double maxNumber)
    {
        return (new System.Random().NextDouble() * (maxNumber - minNumber) + minNumber);
    }

    IEnumerator Shoot()
    {
        if (currentBulletsInMag > 0 && isReloading == false)
        {
            if (Time.time - lastFired > 1 / firingSpeed)
            {
                currentBulletsInMag--;
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
    IEnumerator Reload()
    {
        if (magAmount > 0)
        {
            isReloading = true;
            // moving to/from this position should be gradual
            transform.position = parent.position;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            animator.Play("TommyReload");
            // should be the length of the animation
            yield return new WaitForSeconds(2f);
            magAmount--;
            currentBulletsInMag = MaxBulletsInMag;
            isReloading = false;
        }
    }
}
