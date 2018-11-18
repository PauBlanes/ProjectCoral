using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour {

    private bool alive;

    public enum Type { HorizontalRandom, Vertical, HorizontalToLeft, HorizontalToRight };
    public Type movementType;

    public int speed;

    private SpriteRenderer sprFish;

    private SpriteRenderer sprRendererBG;

    private Vector3 movement = Vector3.zero;

    // Use this for initialization
    void Start () {

        sprFish = GetComponent<SpriteRenderer>();

        sprRendererBG = GameObject.FindGameObjectWithTag("BG").GetComponent<SpriteRenderer>();

        alive = true;

        if (movementType == Type.HorizontalRandom) movementType = (Random.value < 0.5f) ? Type.HorizontalToLeft : Type.HorizontalToRight;

        Vector3 initPos = transform.position;

        if (movementType == Type.HorizontalToRight) {

            if(transform.rotation != Quaternion.identity) {
                sprFish.flipY = true;
            } else {
                sprFish.flipX = true;
            }
            
            initPos.x = -(sprRendererBG.size.x / 2 + sprFish.size.x / 2);
            initPos.y = Random.Range(-(sprRendererBG.size.y / 2 - sprFish.size.y / 2), sprRendererBG.size.y / 2 - sprFish.size.y / 2);

        } else if (movementType == Type.HorizontalToLeft) {

            initPos.x = sprRendererBG.size.x / 2 + sprFish.size.x / 2;
            initPos.y = Random.Range(-(sprRendererBG.size.y / 2 - sprFish.size.y / 2), sprRendererBG.size.y / 2 - sprFish.size.y / 2);

        } else {

            initPos.y = -(sprRendererBG.size.y / 2 + sprFish.size.y / 2);
            initPos.x = Random.Range(-(sprRendererBG.size.x / 2 - sprFish.size.x / 2), sprRendererBG.size.x / 2 - sprFish.size.x / 2);

        }

        speed = Random.Range(speed - speed / 2, speed + speed / 2);

        transform.position = initPos;
		
	}
	
	// Update is called once per frame
	void Update () {

        if (alive) {
            switch (movementType) {
                case Type.HorizontalToLeft:
                    if (transform.position.x + sprFish.size.x / 2 < -(sprRendererBG.size.x / 2)) alive = false;
                    movement.x = -speed * Time.deltaTime;
                    break;

                case Type.HorizontalToRight:
                    if (transform.position.x - sprFish.size.x / 2 > sprRendererBG.size.x / 2) alive = false;
                    movement.x = speed * Time.deltaTime;
                    break;

                case Type.Vertical:
                    if (transform.position.y - sprFish.size.y / 2 > sprRendererBG.size.y / 2) alive = false;
                    movement.y = speed * Time.deltaTime;
                    break;
            }
        } else {
            Destroy(this.gameObject);
        }

        transform.position += movement;
		
	}
}
