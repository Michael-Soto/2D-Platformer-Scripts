using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject[] spawnPoints;
    public GameObject dragon; //link to the prefab
    public bool playerDied;
    public static GameManager instance = null;

    private GameObject[] dragons;
	void Awake () {
        // the following logic follows our singleton pattern
        if (instance == null)
            instance = this;
        else if (instance != this) //someone it creating another game manager... not allowed!
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        // create the enemies when the game starts
        SpawnEnemy();   
    }

    void Update()
    {
        // make more enemies if there are none
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            SpawnEnemy();
    }

    void SpawnEnemy()
    {
        // find where the enemies should spawn
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints");
        dragons = new GameObject[spawnPoints.Length];

        // walk through the spawn points and create an enemy at each point
        int i = 0;
        foreach(GameObject spawnPoint in spawnPoints)
        { 
            dragons[i++]=Instantiate(dragon, spawnPoint.transform);
        }
    }

    

    // control all the events that happen when a player dies
    public void PlayerDeath()
    {
        playerDied = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) // if player still exists
        {
            player.GetComponent<PlayerController>().Die();
            player.GetComponent<AudioSource>().Play();
        }

        // make each dragon respond to death
        foreach (GameObject aDragon in dragons)
            if(aDragon != null) //make sure dragon has not been destroyed
                aDragon.SendMessage("PlayerDeath");
        
        // restarts the game in 3 seconds -- provides time for sounds and animation before restarting
        Invoke("RestartGame", 3f);

    }

    private void RestartGame()
    {
        //restart scene
        SceneManager.LoadScene(0);
        //reset player status so the game can resume
        playerDied = false;
    }
}
