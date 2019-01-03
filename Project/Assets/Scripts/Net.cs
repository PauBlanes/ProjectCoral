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
            float step = 3 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), step);
            if (Mathf.Approximately(transform.position.y, targetY))
            {
                StartCoroutine(WaitAndStartMoving());                    
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

    IEnumerator WaitAndStartMoving()
    {
        yield return new WaitForSeconds(2);
        state = States.moving;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "CORAL" || collision.tag.Contains("FISH")) && state == States.moving)
        {

            Camera.main.GetComponent<EcosystemManager>().UpdateSystemHealth(-5);

            if (collision.gameObject.GetComponent<HealthSystem>() != null)
                collision.gameObject.GetComponent<HealthSystem>().UpdateHealth(-5);

            //Fish
            if (collision.gameObject.GetComponent<FishMovement>() != null)
            {
                collision.gameObject.GetComponent<FishMovement>().captured = true; //pq no es mogui
                collision.transform.SetParent(transform); //que segueixi a la reixa
            }           
        }
    }
}
