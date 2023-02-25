using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{

    private Animator anim;
    private int direction;
    private RaycastHit2D hit;
    private RaycastHit2D hitLeft;
    private RaycastHit2D hitRight;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, 0.9f);
        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.5f);
        hitRight = Physics2D.Raycast(transform.position, Vector2.right, 0.5f);
        //Debug.DrawRay(transform.position, new Vector2(0, -0.9f), Color.red, 1f);

        if (hit.collider == null) //if were are about to walk off the platform
        {
            direction = direction * -1; //change direction
            
        }

        //is the player touching the enemy 
        if (hitLeft.collider != null && !GameManager.instance.playerDied)
            if (hitLeft.collider.gameObject.CompareTag("Player"))
                // kill off player 
                GameManager.instance.PlayerDeath();
            else if (hitLeft.collider.gameObject.CompareTag("Fire"))
                // player was hit by the fire
                Die();

        if (hitRight.collider != null && !GameManager.instance.playerDied)
            if (hitRight.collider.gameObject.CompareTag("Player"))
                // kill off player 
                GameManager.instance.PlayerDeath();
            else if (hitRight.collider.gameObject.CompareTag("Fire"))
                // player was hit by the fire
                Die();

        // make the dragon move if the player is still alive
        if (!GameManager.instance.playerDied)
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x - 1 * direction, transform.position.y), Time.deltaTime);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
            Destroy(gameObject);
    }

    public void PlayerDeath()
    {
        //set dragon to respong to player's death
        anim.SetBool("PlayerDead", true);
    }
    private void Die()
    {
        //play dragon death sound and then destroy dragon object
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, 0.3f);
    }
}
