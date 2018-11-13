using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Vector2 mousePosition, movement;

    private Camera cam;

    private float camWidth, camHeight, maxX, minX, maxY, minY;

    public int speed;

    private SpriteRenderer sprRendererBG;

    // Use this for initialization
    void Start ()
    {
        sprRendererBG = GameObject.FindGameObjectWithTag("BG").GetComponent<SpriteRenderer>();

        cam = GetComponent<Camera>();
        
        camHeight = cam.orthographicSize * 2f;
        camWidth = camHeight * cam.aspect;

        maxX = sprRendererBG.size.x / 2 - camWidth / 2;
        minX = -(sprRendererBG.size.x / 2) + camWidth / 2;

        maxY = sprRendererBG.size.y / 2 - camHeight / 2;
        minY = -(sprRendererBG.size.y / 2) + camHeight / 2;

    }
	
	// Update is called once per frame
	void Update () {

        GetInputs();

        if (mousePosition.y > camHeight/2 - 1)
        {
            movement.y = speed * Time.deltaTime;
        }
        else if (mousePosition.y < -(camHeight/2 - 1))
        {
            movement.y = -speed * Time.deltaTime;
        }

        if (mousePosition.x > camWidth/2 - 1)
        {
            movement.x = speed * Time.deltaTime;
        }
        else if (mousePosition.x < -(camWidth/2 - 1))
        {
            movement.x = -speed * Time.deltaTime;
        }

        movement = new Vector2(Mathf.Clamp(transform.position.x + movement.x, minX, maxX), 
            Mathf.Clamp(transform.position.y + movement.y, minY, maxY));

        transform.position = new Vector3(movement.x, movement.y, transform.position.z);

        movement = Vector2.zero;

    }

    void GetInputs() {

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

    }
}
