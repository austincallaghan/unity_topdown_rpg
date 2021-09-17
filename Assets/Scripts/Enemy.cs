using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	[Header("Movement")]
	public float speed = 3f;
	[Header("Attack")]
	[SerializeField] private float attackDamage = 10f;
	[SerializeField] private float attackSpeed = 1f;
	[SerializeField] Animator animator;
	private float canAttack;
	private float rotationSpeed = 10f;

	[Header("Health")]
	private float health;
	[SerializeField] private float maxHealth;

	private Transform target;

	private void Start() {
		health = maxHealth;
		animator = gameObject.GetComponent<Animator>();             
	}

	public void TakeDamage(float dmg) {
		health -= dmg;

		if (health <= 0) {
			if (animator != null && animator.isActiveAndEnabled) {
				Debug.Log("Enemy Health: " + health);
				animator.SetTrigger("enemyDied");
				speed = 0f;
				rotationSpeed = 0f;
				Destroy(gameObject, 4f);
             }
		}
	}


 	private void RotateTowards(Vector2 target){
 	    var offset = 265f;
 	    Vector2 direction = target - (Vector2)transform.position;
 	    direction.Normalize();
 	    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;       
 	    Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
 		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
 	}

	private void FixedUpdate() {
		if (target != null) {
			float step = speed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, target.position, step);
			RotateTowards(target.position);
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			if (attackSpeed <= canAttack) {
				other.gameObject.GetComponent<PlayerHealth>().UpdateHealth(-attackDamage);
				canAttack = 0f;
			} else {
				canAttack += Time.deltaTime;
			}
		}
	}

	private void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			if (attackSpeed <= canAttack) {
				other.gameObject.GetComponent<PlayerHealth>().UpdateHealth(-attackDamage);
				canAttack = 0f;
			} else {
				canAttack += Time.deltaTime;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			target = other.transform;


			if (animator != null && animator.isActiveAndEnabled) {
				animator.SetBool("targetActive", true);
             }
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			target = null;

			if (animator != null && animator.isActiveAndEnabled) {
				animator.SetBool("targetActive", false);
             }
		}
	}
}
