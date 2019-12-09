using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Animator animator;
    // ideally speed of the recoil animation should be tied to the firing speed somehow
    private AnimationClip recoilAnim;
    public Transform firePoint;
    public int damage = 50;
    public LineRenderer lineRenderer;
    public double inaccuracy = 5;
    public bool isAuto = false;
    public double firingSpeed;
    public SpriteRenderer muzzleFlash;
    private bool isAnimating = false;
    public int magAmount;
    public int MaxBulletsInMag;
    public int currentBulletsInMag;
    private float lastFired;
    Transform parent;
    // Update is called once per frame
    private void Start()
    {
        animator = GetComponent<Animator>();
        lineRenderer.useWorldSpace = true;
        parent = transform.parent;
        animator.enabled = false;
    }
    public void Input()
    {
        if (isAnimating == false)
        {

            if (UnityEngine.Input.GetKeyDown("f"))
            {
                animator.enabled = false; // has to be disabled or position will be 0
                transform.parent = null;
            }

            if (UnityEngine.Input.GetMouseButtonDown(0) && isAuto == false)
            {
                StartCoroutine(Shoot());
            }

            if (UnityEngine.Input.GetMouseButton(0) && isAuto == true)
            {
                StartCoroutine(Shoot());
            }
            if (UnityEngine.Input.GetKeyDown("r"))
            {
                StartCoroutine(Reload());
            }
        }

    }
    public double GetRandomNumberInRange(double minNumber, double maxNumber)
    {
        return (new System.Random().NextDouble() * (maxNumber - minNumber) + minNumber);
    }
    public IEnumerator Shoot()
    {
        if (currentBulletsInMag > 0 && isAnimating == false)
        {
            if (Time.time - lastFired > 1 / firingSpeed)
            {
                isAnimating = true;
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
                isAnimating = false;
            }
        }
    }
    public IEnumerator Reload()
    {
        if (magAmount > 0 && isAnimating == false)
        {

            isAnimating = true;
            // moving to/from this position should be gradual. also it doesn't seem to move at all since i split the script

            animator.Play("TommyReload");
            // should be the length of the animation
            yield return new WaitForSeconds(2f);
            magAmount--;
            currentBulletsInMag = MaxBulletsInMag;
            isAnimating = false;
        }
    }
    private void OnMouseOver()
    {
        if (transform.parent == null && UnityEngine.Input.GetKeyDown("e"))
        {
            Vector2 playerLoc = GameObject.Find("Player").transform.position;
            if (Vector2.Distance(transform.position, playerLoc) < 1)
            {
                animator.enabled = true;
                parent = GameObject.Find("WeaponAttach").transform;
                transform.parent = parent;
                transform.position = parent.position;
                transform.rotation = parent.rotation;
                parent.GetComponent<WeaponAim>().weapon = gameObject;
            }
        }
        
    }
}
