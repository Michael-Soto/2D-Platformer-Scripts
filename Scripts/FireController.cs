using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour {

    private float speed;
    private Vector2 direction;
    private Rigidbody2D rgdbdy;
    public GameObject player;

	// Use this for initialization
	void Start () {
        speed = 4f;
        player = GameObject.FindGameObjectWithTag("Player");
        direction = player.GetComponent<PlayerController>().GetDirection();
        rgdbdy = GetComponent<Rigidbody2D>();
        rgdbdy.velocity = speed * direction;
        Invoke("Die", 1f);
	}

    //destroy whatever the bullet collides with 
    void Die()
    {
        if(gameObject != null)
            Destroy(gameObject);
    }

    // this handles the collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
            Die();
    }

}
