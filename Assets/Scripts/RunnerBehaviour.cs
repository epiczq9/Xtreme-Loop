using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerBehaviour : MonoBehaviour
{
    public GameObject gameController;

    private void Start() {
        gameController = GameObject.Find("GameController");
    }
    public void Jump() {
        GetComponentInChildren<Animator>().StopPlayback();
        if (gameController.GetComponent<GameController>().runFaster) {
            GetComponentInChildren<Animator>().Play("FastJump");
            Debug.Log("Faster Jump Baby");
        } else {
            GetComponentInChildren<Animator>().Play("Jump");
            Debug.Log("Slow Jump");
        }
    }
}
