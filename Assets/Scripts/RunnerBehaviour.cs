using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timers;

public class RunnerBehaviour : MonoBehaviour
{
    public GameObject gameController;
    public GameObject trail;

    private void Start() {
        gameController = GameObject.Find("GameController");
        TimersManager.SetTimer(this, 0.2f, ActivateTrail);
    }

    void ActivateTrail() {
        trail.SetActive(true);
    }

    public void Jump() {
        GetComponentInChildren<Animator>().StopPlayback();
        GetComponentInChildren<Animator>().SetTrigger("Jump");
    }
}
