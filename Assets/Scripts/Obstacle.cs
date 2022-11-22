using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameController gameCon;

    private void OnTriggerEnter(Collider other) {
        switch (other.gameObject.tag) {
            case "Runner":
                gameCon.GetComponent<GameController>().money += 50;
                Debug.Log("Runner Passed");
                break;

            case "Skater":
                gameCon.GetComponent<GameController>().money += 100;
                Debug.Log("Skater Passed");
                break;
        }

        gameCon.GetComponent<GameController>().UpdateMoneyText();
    }
}
