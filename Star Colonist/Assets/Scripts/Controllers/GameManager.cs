using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class GameManager : MonoBehaviour
{
    public static GameState gameState;
    public static WorldGenerator world;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if(instance !=null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            world = this.GetComponent<WorldGenerator>();
            Debug.Log("World is ready to be generated");
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            world.CreateWorld();
        }
    }
}
