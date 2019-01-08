using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Vector2 movement;

    private float maxX, minX, maxY, minY;

    public int speedMagnitude;
    public int zoomSpeed;

    private Vector3 speed = Vector3.zero;
    

    // Use this for initialization
    void Start ()
    {
        //calcular bordes de la pantalla                
        maxX = GameObject.FindGameObjectWithTag("RBorder").transform.position.x;
        minX = GameObject.FindGameObjectWithTag("LBorder").transform.position.x;

        maxY = GameObject.FindGameObjectWithTag("TBorder").transform.position.y;
        minY = GameObject.FindGameObjectWithTag("DBorder").transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {

        //Fer zoom
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float camSize = Camera.main.orthographicSize;
            camSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            camSize = Mathf.Clamp(camSize, 4, 10);
            Camera.main.orthographicSize = camSize;
        }

        //Recalculem els bordes de la camera
        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;
                
        //Setegem nova pos
        speed.x = Input.GetAxis("Horizontal") * speedMagnitude * Time.deltaTime;
        speed.y = Input.GetAxis("Vertical") * speedMagnitude * Time.deltaTime;
        Vector3 newPos = transform.position + speed;
                        
        //Clampejar nova posicio              
        newPos.x = Mathf.Clamp(newPos.x, minX + horzExtent, maxX - horzExtent);
        newPos.y = Mathf.Clamp(newPos.y, minY + vertExtent, maxY - vertExtent);

        //Setejar la pos
        transform.position = newPos;
    }
   
}
