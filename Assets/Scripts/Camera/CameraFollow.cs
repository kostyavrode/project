using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    [SerializeField] private ActiveVectors activeVectors;

    [SerializeField] private GameObject followTarget;
    private Vector3 offset;
    private Vector3 changePos;                              

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        changePos = transform.position;
    }
    public void SetTarget(GameObject target)
    {
        followTarget = target;
        changePos = transform.position;
    }
    private void LateUpdate()
    {
        if (followTarget)
        {
            if (activeVectors.x)
            {
                changePos.x = followTarget.transform.position.x - offset.x;
            }
            if (activeVectors.y)
            {
                changePos.y = followTarget.transform.position.y - offset.y;
            }
            if (activeVectors.z)
            {
                changePos.z = followTarget.transform.position.z - offset.z;
            }
            transform.position = changePos;
        }
    }
}

[System.Serializable]
public class ActiveVectors
{
    public bool x, y, z;
}
