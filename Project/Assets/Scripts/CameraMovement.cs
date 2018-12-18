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
        
        if(Input.GetKey(KeyCode.W))
            movement.y = speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            movement.y = -speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            movement.x = -speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            movement.x = speed * Time.deltaTime;

        /*float maxMouseX = Mathf.Clamp(mousePosition.x, -12, 12);
        float maxMouseY = Mathf.Clamp(mousePosition.y, -6, 6);
        mousePosition = new Vector2(maxMouseX, maxMouseY);*/

        /*if (mousePosition.y > camHeight/2 - 15)
        {
            movement.y = speed * Time.deltaTime;
        }
        else if (mousePosition.y < -(camHeight/2 - 15))
        {
            movement.y = -speed * Time.deltaTime;
        }*/

        /*if (mousePosition.x > camWidth/2 - 10)
        {
            movement.x = speed * Time.deltaTime;
        }
        else if (mousePosition.x < -(camWidth/2 - 200))
        {
            movement.x = -speed * Time.deltaTime;
        }*/

        movement = new Vector2(Mathf.Clamp(transform.position.x + movement.x, minX, maxX), 
            Mathf.Clamp(transform.position.y + movement.y, minY, maxY));

        transform.position = new Vector3(movement.x, movement.y, transform.position.z);

        movement = Vector2.zero;

    }

    void GetInputs() {

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

    }
}
