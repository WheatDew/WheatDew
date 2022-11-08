using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyWhaleAI : MonoBehaviour
{
    public Vector3 bound;
    public Vector3 centerPoint;
    public Vector3 maxDistance;
    public Vector3 minDistance;
    public float speed=1;
    private Vector3 targetPoint;
    private float progress=0;
    private Vector3 startPoint;
    private Quaternion startRotation;

    private void Start()
    {
        targetPoint=transform.position;
        startPoint = transform.position;
        startRotation = transform.rotation;
        NewTargetPoint();
    }

    private void Update()
    {
        if (progress* speed > 1)
        {
            NewTargetPoint();
            progress = 0;
            startPoint=transform.position;
            startRotation = transform.rotation;
        }
        progress += Time.deltaTime;
        Vector3 nextPosition = Vector3.Lerp(startPoint, targetPoint, progress*speed);
        Quaternion nextRotation = Quaternion.Lerp(startRotation, Quaternion.LookRotation(targetPoint-transform.position), progress * speed);

        transform.position = nextPosition;
        transform.rotation = nextRotation;
    }

    public async void NewTargetPoint()
    {

        Vector3 p = transform.position;
        float rx = GetRandomValue(minDistance.x, maxDistance.x, p.x);
        float ry = GetRandomValue(minDistance.y, maxDistance.y, p.y);
        float rz = GetRandomValue(minDistance.z, maxDistance.z, p.z);

        if (IsTargetPointInBound(new Vector3(rx, ry, rz)))
        {
            targetPoint = new Vector3(rx, ry, rz);
            return;
        }
        await new WaitForUpdate();
        NewTargetPoint();
    }

    public bool IsTargetPointInBound(Vector3 targetPoint)
    {
        if (targetPoint.x > centerPoint.x + bound.x)
            return false;
        if (targetPoint.x < centerPoint.x - bound.x)
            return false;
        if (targetPoint.y > centerPoint.y + bound.y)
            return false;
        if (targetPoint.y < centerPoint.y - bound.y)
            return false;
        if (targetPoint.x > centerPoint.x + bound.x)
            return false;
        if (targetPoint.x < centerPoint.x - bound.x)
            return false;
        return true;
    }

    public float GetRandomValue(float min,float max,float center)
    {
        float temp=Random.Range(min,max);
        return Random.value>0?temp:-temp;
    }
}
