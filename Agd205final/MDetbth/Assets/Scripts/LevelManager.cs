using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance { set; get;
    }
    private int hitpoint = 3;
    private int score = 0;

    public Transform spawnPostion;
    public Transform playerTransform;

    private void Awake()
    {
        Instance = this;
    }

    // Every single frame
    private void Update()
    {
        if(playerTransform.position.y < -10)
        {
            playerTransform.position = spawnPostion.position;
            hitpoint--; // hitpoint = hitpoint - 1;
            if(hitpoint <= 0)
            {
                Debug.Log("Deaht");
            }
        }
    }

    public void Win()
    {
        Debug.Log("Victiory");
    }

}
