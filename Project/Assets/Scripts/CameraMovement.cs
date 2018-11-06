using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Vector2 mousePosition, movement;

    private Camera cam;

    // Use this for initialization
    void Start () {

        cam = GetComponent<Camera>();

    }
	
	// Update is called once per frame
	void Update () {

        GetInputs();

        if (mousePosition.y > cam.orthographicSize - 1) {
            movement.y = transform.position.y + 10 * Time.deltaTime;
        }else if (mousePosition.y < -(cam.orthographicSize - 1)) {
            movement.y = transform.position.y - 10 * Time.deltaTime;
        }

        if (mousePosition.x > cam.orthographicSize * 2) {
            movement.x = transform.position.x + 10 * Time.deltaTime;
        } else if (mousePosition.x < -(cam.orthographicSize * 2)) {
            movement.x = transform.position.x - 10 * Time.deltaTime;
        }

        transform.position = new Vector3(movement.x, movement.y, transform.position.z);

    }

    void GetInputs() {

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

    }
}
