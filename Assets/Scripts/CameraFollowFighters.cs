using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollowFighters : MonoBehaviour
{
    List<Transform> targets = new List<Transform>();
    [SerializeField]private Vector3 offset;
    private Vector3 velocity;
    [SerializeField] private float smoothTime = 0.5f;
    [SerializeField] private float minZoom = 60f;
    [SerializeField] private float maxZoom = 30f;
    [SerializeField] private float zoomLimiter = 20f;
    private Bounds targetBounds;
    private Camera stageCamera;
    private Transform stageFocalPoint;

    private void Start()
    {
        stageCamera = GetComponent<Camera>();
        stageFocalPoint = GameObject.FindGameObjectWithTag("StageFocalPoint").transform;
    }
    private void Update()
    {
        DetectFighters();
    }

    private void DetectFighters()
    {
        //We add the focal point of the stage to the targetList
        targets.Clear();
        targets.Add(stageFocalPoint);
        var fighters = FindObjectsOfType<Fighter>();
        foreach(var fighter in fighters)
        {
            targets.Add(fighter.transform);
        }
    }

    private Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }
        targetBounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (var target in targets)
        {
            targetBounds.Encapsulate(target.position);
        }
        return targetBounds.center;
    }

    private void LateUpdate()
    {
        Move();
        Zoom();
    }

    private void Move()
    {
        if (targets.Count == 0)
        {
            return;
        }
        var centerPoint = GetCenterPoint();
        var newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance()/ zoomLimiter);
        stageCamera.fieldOfView = Mathf.Lerp(stageCamera.fieldOfView,newZoom,Time.deltaTime);
    }

    private float GetGreatestDistance()
    {
        return targetBounds.size.x;
    }
}
