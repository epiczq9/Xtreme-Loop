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
        GetComponentInChildren<Animator>().SetTrigger("Jump");
    }
}
