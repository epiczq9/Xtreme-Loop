using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public GameObject runnerPrefab;
    public GameObject skaterPrefab;

    public PathCreator runnerPath;
    public PathCreator skaterPath;

    public List<GameObject> runnersList;
    public List<GameObject> skatersList;

    public GameObject newRunnerButton;
    public GameObject newSkaterButton;
    public GameObject newObstacleButton;

    public GameObject skaterCourse;

    int currentlyActiveObstacles = 1;
    public GameObject[] obstacles;

    public Transform spawnLocation;

    public Text runnerPriceText;
    public Text skaterPriceText;
    public Text obstaclePriceText;

    float baseSpeed;
    public int money = 0;
    public float runnerPrice = 100;
    public int skaterPrice = 1000;
    public int obstaclePrice = 500;

    public bool timeSpedUp = false;

    public bool runFaster = false;
    public bool spedUp = false;
    float tapTimerMax = 3f;
    float tapTimerCurrent = 0f;

    public Text moneyText;

    public CinemachineVirtualCamera runnerPathCam;
    public CinemachineVirtualCamera skaterPathCam;

    void Start() {
        UpdateMoneyText();
        baseSpeed = runnersList[0].GetComponent<PathCreation.Examples.PathFollower>().speed;
    }

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            tapTimerCurrent = 0;
            Time.timeScale = 2;
            timeSpedUp = true;
            /*
            runFaster = true;
            if (!spedUp) {
                SpeedUpEveryone();
                spedUp = true;
            }
            */
        }

        if (timeSpedUp) {
            if (tapTimerCurrent < tapTimerMax) {
                tapTimerCurrent += Time.deltaTime;
            } else {
                timeSpedUp = false;
                Time.timeScale = 1;
        }

            /*if (runFaster) {
                if (tapTimerCurrent < tapTimerMax) {
                    tapTimerCurrent += Time.deltaTime;
                } else {
                    runFaster = false;
                    ResetSpeed();
                }*/
        }

        if (runnersList.Count < 8 && money >= runnerPrice) {
            newRunnerButton.GetComponent<Button>().interactable = true;
        } else {
            newRunnerButton.GetComponent<Button>().interactable = false;
        }

        if (runnersList.Count > 3 && skatersList.Count < 8 && money >= skaterPrice) {
            newSkaterButton.GetComponent<Button>().interactable = true;
        } else {
            newSkaterButton.GetComponent<Button>().interactable = false;
        }

        if(money >= obstaclePrice && currentlyActiveObstacles < obstacles.Length) {
            newObstacleButton.GetComponent<Button>().interactable = true;
        } else {
            newObstacleButton.GetComponent<Button>().interactable = false;
        }
    }

    public void SpawnNewRunner() {
        GameObject newRunner = Instantiate(runnerPrefab, spawnLocation);
        if (runFaster) {
            newRunner.GetComponent<PathCreation.Examples.PathFollower>().speed = baseSpeed * 2f;
        }
        newRunner.GetComponent<PathCreation.Examples.PathFollower>().pathCreator = runnerPath;
        newRunner.GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled =
            runnersList[0].GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled - NextRunnerPosition();
        Debug.Log(NextRunnerPosition());
        runnersList.Add(newRunner);
        money -= (int)runnerPrice;
        runnerPrice = (int)(runnerPrice * 1.15f);
        UpdateMoneyText();
    }

    public float NextRunnerPosition() {
        return runnersList.Count switch {
            1 => runnerPath.path.length / 2,
            2 => runnerPath.path.length / 4,
            3 => runnerPath.path.length * 3 / 4,
            4 => runnerPath.path.length / 8,
            5 => runnerPath.path.length * 5 / 8,
            6 => runnerPath.path.length * 3 / 8,
            7 => runnerPath.path.length * 7 / 8,
            _ => 0,
        };
    }

    public void MergeRunners() {
        if(runnerPathCam.Priority > skaterPathCam.Priority) {
            runnerPathCam.Priority = 10;
            skaterPathCam.Priority = 20;
            skaterCourse.SetActive(true);
            if (!obstacles[4].activeInHierarchy) {
                obstacles[4].SetActive(true);
            }
        }

        for (int i = 0; i < 3; i++) {
            Destroy(runnersList[runnersList.Count - 1]);
            runnersList.RemoveAt(runnersList.Count - 1);
        }
        GameObject newSkater = Instantiate(skaterPrefab, spawnLocation);
        newSkater.GetComponent<PathCreation.Examples.PathFollower>().pathCreator = skaterPath;
        if (skatersList.Count == 0) {
            newSkater.GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled = 0;
        } else {
            newSkater.GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled
            = skatersList[0].GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled - NextSkaterPosition();
        }
        skatersList.Add(newSkater);
        money -= skaterPrice;
        skaterPrice = (int)(skaterPrice * 1.15f);
        UpdateMoneyText();
    }

    public float NextSkaterPosition() {
        return skatersList.Count switch {
            1 => skaterPath.path.length / 2,
            2 => skaterPath.path.length / 4,
            3 => skaterPath.path.length * 3 / 4,
            4 => skaterPath.path.length / 8,
            5 => skaterPath.path.length * 5 / 8,
            6 => skaterPath.path.length * 3 / 8,
            7 => skaterPath.path.length * 7 / 8,
            _ => 0,
        };
    }

    public void UpdateMoneyText() {
        moneyText.text = money.ToString();
        runnerPriceText.text = runnerPrice.ToString();
        skaterPriceText.text = skaterPrice.ToString();
        obstaclePriceText.text = obstaclePrice.ToString();
    }

    void SpeedUpEveryone() {
        foreach (GameObject runner in runnersList) {
            runner.GetComponent<PathCreation.Examples.PathFollower>().speed = baseSpeed * 2f;
            runner.GetComponentInChildren<Animator>().Play("FastRun");
        }
        foreach (GameObject skater in skatersList) {
            skater.GetComponent<PathCreation.Examples.PathFollower>().speed = baseSpeed * 2f;
        }
    }
    void ResetSpeed() {
        foreach (GameObject runner in runnersList) {
            runner.GetComponent<PathCreation.Examples.PathFollower>().speed = baseSpeed;
            runner.GetComponentInChildren<Animator>().Play("Run");
        }
        foreach (GameObject skater in skatersList) {
            skater.GetComponent<PathCreation.Examples.PathFollower>().speed = baseSpeed;
        }
        spedUp = false;
    }

    public void ActivateObstacle() {
        if (obstacles[currentlyActiveObstacles].activeInHierarchy) {
            currentlyActiveObstacles++;
        }
        obstacles[currentlyActiveObstacles].SetActive(true);
        currentlyActiveObstacles++;
        money -= obstaclePrice;
        obstaclePrice = (int)(obstaclePrice * 1.15f);
        UpdateMoneyText();
    }

}
