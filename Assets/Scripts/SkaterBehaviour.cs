using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkaterBehaviour : MonoBehaviour
{
    public GameObject gameController;
    public GameObject skater;
    public GameObject board;
    private void Start() {
        gameController = GameObject.Find("GameController");
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
