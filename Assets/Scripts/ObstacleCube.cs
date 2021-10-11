using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCube : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            StartCoroutine(GameManager.instance.IsObstacle(this.gameObject, transform.position));
            //else
            //Destroy(this.gameObject);
        }
    }
}
