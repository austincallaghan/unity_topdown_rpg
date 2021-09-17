using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed;
    private float rotationSpeed = 10f;


    public Rigidbody2D rb;
	[SerializeField] Transform player;

    Vector2 movement;

    private float activeMoveSpeed;
    public float dashSpeed = 10f; 

    public float dashLength = .3f, dashCooldown = 0f;

    private float dashCounter;
    private float dashCoolCounter;

    void Start() {
        activeMoveSpeed = moveSpeed;
    }

    void Update() {
        MovementInput();
		RotatePlayer();
    }

    private void FixedUpdate() {
        // rb.velocity = movement * moveSpeed;
        rb.velocity = movement * activeMoveSpeed;

    }

	void RotatePlayer() {
		float angle = Utility.AngleTowardsMouse(player.position);
		player.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
	}

    void MovementInput () {
        float mx = Input.GetAxisRaw("Horizontal");
        float my = Input.GetAxisRaw("Vertical");

        movement = new Vector2(mx, my).normalized;

        if(Input.GetKeyDown(KeyCode.Space)) {
            if(dashCoolCounter <= 0 && dashCounter <= 0) {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
            }
        }

        if(dashCounter > 0) {
            dashCounter -= Time.deltaTime;
            if(dashCounter <= 0) {
                activeMoveSpeed = moveSpeed;
                dashCoolCounter = dashCooldown;
            }
        }

        if(dashCoolCounter > 0) {
            dashCoolCounter -= Time.deltaTime;
        }
    }
}
