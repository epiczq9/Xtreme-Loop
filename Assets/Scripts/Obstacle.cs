using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Obstacle : MonoBehaviour
{
    public GameController gameCon;
    public GameObject points;
    public GameObject uiSpawn;

    int runnerReward = 50;
    int rollerSkaterReward = 100;
    private void OnTriggerEnter(Collider other) {
        switch (other.gameObject.tag) {
            case "Runner":
                gameCon.GetComponent<GameController>().money += runnerReward;
                ShowPoints(runnerReward);
                Debug.Log("Runner Passed");
                break;

            case "Rollerskater":
                gameCon.GetComponent<GameController>().money += rollerSkaterReward;
                ShowPoints(rollerSkaterReward);
                Debug.Log("Skater Passed");
                break;
        }

        gameCon.GetComponent<GameController>().UpdateMoneyText();
    }

    void ShowPoints(int amount) {
        GameObject rewardText = uiSpawn.GetComponent<UISpawn>().SpawnPointUI(points);
        rewardText.GetComponent<TMP_Text>().text = "+ " + amount.ToString();
    }
}
