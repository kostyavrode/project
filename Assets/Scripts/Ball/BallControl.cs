using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BallControl : MonoBehaviour
{
    public static BallControl instance;

    [SerializeField] private TrajectoryController trajectoryController;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float maxForce;
    [SerializeField] private float forceModifier = 0.5f;
    [SerializeField] private GameObject areaAffector;
    [SerializeField] private LayerMask rayLayer;
    [SerializeField] private float timeOffTheFlight = 2;
    [Header("Ball Shoot Settings")]
    [Range(0, 10)]
    [SerializeField] private float golfClubType1ShootYVallue=3;
    [Range(0, 10)]
    [SerializeField] private float golfClubType2ShootYVallue = 5;
    [Range(0, 10)]
    [SerializeField] private float golfClubType3ShootYVallue = 0.5f;


    private float force;
    private int golfClubType=2;
    private Rigidbody rgBody;

    private Vector3 lastPos;
    private Vector3 startPos, endPos;
    private bool canShoot = false, ballIsStatic = true;
    private Vector3 direction;
    private List<Vector3> linePoints = new List<Vector3>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        rgBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        lastPos = transform.position;
    }
    private void Update()
    {
        if (rgBody.linearVelocity == Vector3.zero && !ballIsStatic)
        {
            ballIsStatic = true;
            rgBody.angularVelocity = Vector3.zero;
            areaAffector.SetActive(true);
            lastPos = transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (canShoot)
        {
            Shoot();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Water")
        {
            StartCoroutine(WaitToTransformToLastPos());
            Debug.Log("watyer");
        }
    }
    private void Shoot()
    {
        canShoot = false;
        ballIsStatic = false;
        rgBody.AddForce(GetShootDirectionVector(), ForceMode.Impulse);
        areaAffector.SetActive(false);
        force = 0;
        startPos = endPos = Vector3.zero;
    }
    public void SetGolfClubType(int type)
    {
        golfClubType = type;
    }
    public void MouseDownMethod()
    {
        if(!ballIsStatic) return;
        startPos = ClickedPoint();
        lineRenderer.gameObject.SetActive(true);
    }

    public void MouseHoldMethod()
    {
        if (!ballIsStatic) return;
        endPos = ClickedPoint();
        Vector3 dir = startPos - endPos;
        //trajectoryController.velocity = new Vector3(dir.x, 5, dir.z) * force/10;
        
        trajectoryController.velocity = GetShootDirectionVector()/10;
        //Debug.Log("GetShootDirectionVector=" + GetShootDirectionVector() / 10);
        //Debug.Log("DIR=" + new Vector3(dir.x, 5, dir.z) * force / 10);
    }
    public void MouseUpMethod()
    {
        if(!ballIsStatic) return;
        canShoot = true;
        lineRenderer.gameObject.SetActive(false);
        linePoints.Clear();
        lineRenderer.SetPositions(linePoints.ToArray());
    }
    public void ResetVelocity()
    {
        rgBody.isKinematic = true;
        rgBody.useGravity = false;
        rgBody.isKinematic = false;
        rgBody.useGravity = true;
    }
    Vector3 ClickedPoint()
    {
        Vector3 position = Vector3.zero;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, rayLayer))
        {
            position = hit.point;
        }
        return position;
    }
    private Vector3 GetShootDirectionVector()
    {
        force = Mathf.Clamp(Vector3.Distance(endPos, startPos) * forceModifier, 0, maxForce);
        direction = startPos - endPos;
        switch (golfClubType)
        {
            case 1:
                return new Vector3(direction.x, 3, direction.z) * force;
            case 2:
                return new Vector3(direction.x/2, 5, direction.z/2) * force;
            case 3:
                return new Vector3(direction.x, 0.5f, direction.z) * force;
            default:
                return Vector3.zero;
        }
    }
    private IEnumerator WaitToTransformToLastPos()
    {
        yield return new WaitForSeconds(1);
        ResetVelocity();
        transform.position = lastPos;
    }
}
