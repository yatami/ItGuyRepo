using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugLevelController : MonoBehaviour
{
    GameObject gameController;

    int plugCountInLevel;
    int connectedPlugCount;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject t in GameObject.FindGameObjectsWithTag("Plug"))
        {
            plugCountInLevel++;
        }
        gameController = GameObject.FindGameObjectWithTag("GameControl");
    }

    // Update is called once per frame
    void Update()
    {
        if(connectedPlugCount == plugCountInLevel)
        {
            gameController.GetComponent<GameController>().levelWon();
        }
    }

    public void incrementConnectedPlugCount()
    {
        connectedPlugCount++;
    }

    public void decreaseConnectedPlugCount()
    {
        connectedPlugCount--;
    }
}
