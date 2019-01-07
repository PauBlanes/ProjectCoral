using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour {

    private bool alive;

    public enum Type { HorizontalRandom, Vertical, HorizontalToLeft, HorizontalToRight };
    public Type movementType;

    public int speed;

    private SpriteRenderer sprFish;    

    private Vector3 movement = Vector3.zero;

    private float wanderAngle = 5;
    private float circleDistance;
    private float circleRadius;
    private float angleChange = 5;

    public bool captured = false;

    //per fer els bordes de la pantall
    struct Borders
    {
        public float left;
        public float right;
        public float top;
        public float down;
    }
    Borders borders;


    // Use this for initialization
    void Start () {

        sprFish = GetComponent<SpriteRenderer>();

        //definir els bordes        
        borders.left = GameObject.FindGameObjectWithTag("LBorder").transform.position.x;
        borders.right = GameObject.FindGameObjectWithTag("RBorder").transform.position.x;
        borders.top = GameObject.FindGameObjectWithTag("TBorder").transform.position.x;
        borders.down = GameObject.FindGameObjectWithTag("DBorder").transform.position.x;

        alive = true;

        if (movementType == Type.HorizontalRandom) movementType = (Random.value < 0.5f) ? Type.HorizontalToLeft : Type.HorizontalToRight;

        Vector3 initPos = transform.position;

        circleDistance = sprFish.size.x * 5f;
        circleRadius = sprFish.size.y / 2;

        if (movementType == Type.HorizontalToRight) {

            if(transform.rotation != Quaternion.identity) {
                sprFish.flipY = true;
            } else {
                sprFish.flipX = true;
            }
            
            initPos.x = borders.left - sprFish.size.x / 2;
            initPos.y = Random.Range(-8, borders.top - sprFish.size.y / 2);

        } else if (movementType == Type.HorizontalToLeft) {

            initPos.x = borders.right + sprFish.size.x / 2;
            initPos.y = Random.Range(-8, borders.top - sprFish.size.y / 2);

        } else {

            initPos.y = borders.down - sprFish.size.y / 2;
            initPos.x = Random.Range((borders.left - sprFish.size.x / 2), borders.right + sprFish.size.x / 2);

            circleDistance = sprFish.size.y * 5f;
            circleRadius = sprFish.size.x / 2;

        }

        speed = Random.Range(speed - speed / 2, speed + speed / 2);

        transform.position = initPos;

    }
	
	// Update is called once per frame
	void Update () {

        if (alive) {

            switch (movementType) {
                case Type.HorizontalToLeft:
                    if (transform.position.x + sprFish.size.x / 2 < borders.left) alive = false;
                    movement.x = -speed * Time.deltaTime;
                    break;

                case Type.HorizontalToRight:
                    if (transform.position.x - sprFish.size.x / 2 > borders.right) alive = false;
                    movement.x = speed * Time.deltaTime;
                    break;

                case Type.Vertical:
                    if (transform.position.y - sprFish.size.y / 2 > borders.top) alive = false;
                    movement.y = speed * Time.deltaTime;
                    break;
            }

            movement = Wander(movement) * speed * Time.deltaTime;

        } else {
            Destroy(this.gameObject);
        }

        if (!captured) {
            transform.position += movement;
        }
		
	}

    private Vector3 Wander(Vector3 move) {

        Vector3 circleCenter = move.normalized * circleDistance;

        Vector3 displacement = new Vector3(0, 1, 0) * circleRadius;
        displacement = setAngle(displacement, wanderAngle);
        wanderAngle += Random.value * angleChange - angleChange * .5f;

        return (circleCenter + displacement).normalized;

    }

    private Vector3 setAngle(Vector3 vec, float angle) {

        float len = vec.magnitude;
        vec.x = Mathf.Cos(angle) * len;
        vec.y = Mathf.Sin(angle) * len;

        return vec;

    }
}
