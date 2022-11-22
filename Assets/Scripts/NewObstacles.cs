using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewObstacles : MonoBehaviour
{
    public int currentlyActiveObstacles = 1;
    public GameObject[] obstacles;

    public void ActivateObstacle() {
        obstacles[currentlyActiveObstacles].SetActive(true);
        currentlyActiveObstacles++;
        GetComponent<GameController>().money -= GetComponent<GameController>().obstaclePrice;
    }
}
