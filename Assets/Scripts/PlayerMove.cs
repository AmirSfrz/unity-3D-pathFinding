using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 20;
    public Vector3[] movementPoints;
    int currentPointIndex;

    public void Set(Vector3[] movementPoints)
    {
        this.movementPoints = movementPoints;
        this.currentPointIndex = movementPoints.Length - 1;
    }

    private void Update()
    {
        if (currentPointIndex >= 0)
        {
            transform.LookAt(movementPoints[currentPointIndex]);
            //Vector3 rotation = transform.eulerAngles;
            //rotation.z = transform.eulerAngles.y * -1;
            //transform.eulerAngles = rotation;
            //float angle=Vector3.SignedAngle(transform.position, movementPoints[currentPointIndex], transform.forward+transform.right);
            float angle = Mathf.Atan2(movementPoints[currentPointIndex].z - transform.position.z, movementPoints[currentPointIndex].x - transform.position.x) * Mathf.Rad2Deg;
            Vector3 rotation = transform.eulerAngles;
            rotation.z = angle;
            transform.eulerAngles = rotation;

            transform.position = Vector3.MoveTowards(transform.position, movementPoints[currentPointIndex], speed * Time.deltaTime);
            if (transform.position == movementPoints[currentPointIndex])
                currentPointIndex--;
        }
        else
            GameManager.instance.statusTxt.text = "target reached goal";
    }
}