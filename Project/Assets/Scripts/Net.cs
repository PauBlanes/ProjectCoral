using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour {

    enum States { goDown, moving };

    States state = States.goDown;
    int targetY;
    int direction;

    public float speed;

    // Use this for initialization
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 20, transform.position.z);
        targetY = 0;

        //triar direccio
        direction = Random.Range(0, 1);
        if (direction == 0)
        {
            direction = -1;
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

    // Update is called once per frame
    void Update () {
        if (state == States.goDown)
        {
            float step = 6 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), step);
            if (Mathf.Approximately(transform.position.y, targetY))
            {
                state = States.moving;                
            }
        }
        else if (state == States.moving)
        {            
            transform.position += new Vector3(direction,0,0) * Time.deltaTime * speed; 
            if (transform.position.x > 23 || transform.position.x < -23)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "CORAL" || collision.tag.Contains("FISH")) && state == States.moving)
        {
            //Coral
            /*Color col = collision.gameObject.GetComponent<SpriteRenderer>().color;
            col.a -= 0.35f;
            collision.gameObject.GetComponent<SpriteRenderer>().color = col;
            if (col.a <= 0)
            {
                col.a = 1;
                Camera.main.GetComponent<EcosystemManager>().HideCoral(collision.gameObject);
            }*/
            if (collision.gameObject.GetComponent<HealthSystem>() != null)
                collision.gameObject.GetComponent<HealthSystem>().UpdateHealth(-35);

            //Fish
            //set captured = true
            //set offset
            //añadir al array
        }
    }
}
