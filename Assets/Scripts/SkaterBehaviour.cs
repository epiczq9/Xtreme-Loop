using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Timers;

public class SkaterBehaviour : MonoBehaviour
{
    public GameObject gameController;
    public GameObject skater;
    public GameObject board;
    public GameObject trail;
    private void Start() {
        gameController = GameObject.Find("GameController");
        TimersManager.SetTimer(this, 0.2f, ActivateTrail);
    }

    void ActivateTrail() {
        trail.SetActive(true);
    }
    public void Jump() {
        //skater.GetComponent<Animator>().StopPlayback();
        skater.GetComponent<Animator>().Play("Jump");
        board.GetComponent<Animator>().Play("Jump");
        MoveUp();
    }

    void MoveUp() {
        Sequence jumpSeq = DOTween.Sequence();
        jumpSeq.Append(transform.DOMoveY(0, 0.5f)).Append(transform.DOMoveY(1.5f, 0.5f)).Append(transform.DOMoveY(0, 0.4f));
    }
}
