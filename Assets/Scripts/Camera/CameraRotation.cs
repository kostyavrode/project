using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private float rotationSpeed = 0.2f;
    [SerializeField] private float timeToChangeGolfClubPosition;
     private Vector3 golfClub1Position;
     private Vector3 golfClub2Position;
     private Vector3 golfClub3Position;
    public static CameraRotation instance;

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
    }
    public void RotateCamera(float XaxisRotation)           
    {
        transform.Rotate(Vector3.down, -XaxisRotation * rotationSpeed);
    }
    public void MoveCameraToGolfClubPosition(int golfClubType)
    {
        switch (golfClubType)
        {
            case 1:
                cameraObject.transform.DOLocalMove(golfClub1Position, timeToChangeGolfClubPosition);
                break;
            case 2:
                cameraObject.transform.DOLocalMove(golfClub2Position, timeToChangeGolfClubPosition);
                break;
            case 3:
                cameraObject.transform.DOLocalMove(golfClub3Position, timeToChangeGolfClubPosition);
                break;
            default: break;
        }
    }
}
