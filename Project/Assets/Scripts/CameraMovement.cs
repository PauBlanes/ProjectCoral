using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Vector2 mousePosition, movement;

    private Camera cam;

    public int speed;

    // Use this for initialization
    void Start () {

        cam = GetComponent<Camera>();

    }
	
	// Update is called once per frame
	void Update () {

        GetInputs();

        if (mousePosition.y > cam.orthographicSize - 1) {
            movement.y = transform.position.y + speed * Time.deltaTime;
        }else if (mousePosition.y < -(cam.orthographicSize - 1)) {
            movement.y = transform.position.y - speed * Time.deltaTime;
        }

        if (mousePosition.x > cam.orthographicSize * 2) {
            movement.x = transform.position.x + speed * Time.deltaTime;
        } else if (mousePosition.x < -(cam.orthographicSize * 2)) {
            movement.x = transform.position.x - speed * Time.deltaTime;
        }

        transform.position = new Vector3(movement.x, movement.y, transform.position.z);

    }

    void GetInputs() {

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

    }
}
