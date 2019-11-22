using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float ViewAngle;

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z - 180; // 180 because it was the wrong way around. fuck if i know
        }
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), -Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


    // Update is called once per frame
    void Update()
    {






        //if (Physics.Linecast(transform.position, target.position))
        //{
        //    Debug.Log("blocked");
        //}
    }
}
