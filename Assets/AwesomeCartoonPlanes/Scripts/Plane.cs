using UnityEngine;
using System.Collections;

// PLEASE NOTE! THIS SCRIPT IS FOR DEMO PURPOSES ONLY :) //

public class Plane : MonoBehaviour {
	public GameObject prop;
	public GameObject propBlured;

    public Camera[] cameras;
    public int currentCamera = 0;

	public bool engenOn;
    private void Start()
    {
        currentCamera = 0; 
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i == currentCamera)
                cameras[i].gameObject.SetActive(true);
            else
                cameras[i].gameObject.SetActive(false);
        }
    }
    void Update () 
	{
		if (engenOn) {
			prop.SetActive (false);
			propBlured.SetActive (true);
			propBlured.transform.Rotate (1000 * Time.deltaTime, 0, 0);
		} else {
			prop.SetActive (true);
			propBlured.SetActive (false);
		}

        if (Input.GetKeyDown(KeyCode.C))
        {
            cameras[currentCamera].gameObject.SetActive(false);
            currentCamera ++;
            currentCamera %= cameras.Length;
            cameras[currentCamera].gameObject.SetActive(true);
        }
	}
}

// PLEASE NOTE! THIS SCRIPT IS FOR DEMO PURPOSES ONLY :) //