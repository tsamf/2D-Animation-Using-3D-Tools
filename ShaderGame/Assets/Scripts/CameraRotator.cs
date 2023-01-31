using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour {

    public float rotationSpeed;
	
	void Update () {
        gameObject.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
