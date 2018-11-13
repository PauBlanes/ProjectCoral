using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour {

    private bool alive;

    public enum Type { HorizontalToLeft, HorizontalToRight, Vertical };
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

        if (movementType == Type.HorizontalToRight) sprFish.flipX = true;
		
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
