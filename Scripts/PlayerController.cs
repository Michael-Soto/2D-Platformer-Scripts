using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rgbdy;
    private Animator anim;
    private bool facingRight;
    private RaycastHit2D hit;
    private bool canJump;
    private float nextFire;
    private float fireRate = 0.3f;

    public Transform firePoint; //to get where the fire should appear
    public GameObject fire;
    public float moveForce = 365f;
    public float maxSpeed = 7f;
    public float jumpForce = 150f;

    // Use this for initialization
	void Start () {
        rgbdy = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        facingRight = true; //starts facing right
        canJump = false; //not starting in jump
	}

    void Update()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f);
        //Debug.DrawRay(transform.position, new Vector2(0, -0.6f), Color.red, 1f);

        //is the player touching the ground?
        if(Input.GetButtonDown("Jump") && hit.collider != null)
        {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "FloatingPlatform")
            {
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }

        //controls the spawning of bullets
        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            nextFire = Time.time + fireRate;
            anim.SetBool("IsShooting", true);
            Instantiate(fire, firePoint.position, facingRight ? firePoint.rotation: Quaternion.Euler(0, 180, 0));
        }
        else
            anim.SetBool("IsShooting", false);
    }

    // Update in sync with physics engine
    void FixedUpdate()
    {
        // don't run this logic if we are processing the player's death
        if (!GameManager.instance.playerDied)
        {
            float horizontal = Input.GetAxis("Horizontal"); //-1 to 1

            //make sprite match the direction the player is moving in
            if (horizontal > 0 && !facingRight)
                Flip();
            else if (horizontal < 0 && facingRight)
                Flip();

            // control how fast the player can move
            if (horizontal * rgbdy.velocity.x < maxSpeed)
                rgbdy.AddForce(Vector2.right * horizontal * moveForce);

            // but limit how fast the player can move
            if (Mathf.Abs(rgbdy.velocity.x) > maxSpeed)
                rgbdy.velocity = new Vector2(Mathf.Sign(rgbdy.velocity.x) * maxSpeed, rgbdy.velocity.y);

            // we modified this so it does not look like the player is walking on the moving platform
            if (horizontal != 0f) //* Mathf.Abs(horizontal) > 0)
                //trigger animator to change state
                anim.SetBool("IsWalking", true);
            else
                anim.SetBool("IsWalking", false);

            // process the jump
            if (canJump)
            {
                rgbdy.AddForce(new Vector2(0, jumpForce));
                canJump = false;
            }
        }
    }

    // this flips the player's sprite
    void Flip()
    {
        facingRight = !facingRight; //flips boolean value
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1; //flip x value of the scale
        transform.localScale = theScale;
    }

    // If player falls off the platform, process the death
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            //Destroy(gameObject);
            GameManager.instance.PlayerDeath();
        }
    }

    // if the player lands on the platform, make the platform the player's parent to allow it to move with the platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FloatingPlatform"))
        {
            transform.parent = collision.gameObject.transform;
        }

    }

    // once the player leaves the platform, remove the parent association
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FloatingPlatform"))
        {
            transform.parent = null;
        }
    }

    // set the player to play the player death animation
    public void Die()
    {
        anim.SetBool("IsDead", true);
    }

    // return the player's direction (Vector2.right or Vector2.left)
    // this is needed so that we can fire the bullet in the correct direction
    public Vector2 GetDirection()
    {
        if (facingRight)
            return Vector2.right;
        else
            return Vector2.left;
    }

}
