using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public GameObject runnerPrefab;
    public GameObject rollerSkaterPrefab;
    public GameObject skateboarderPrefab;

    public PathCreator runnerPath;
    public PathCreator rollerSkaterPath;
    public PathCreator skateboarderPath;

    public List<GameObject> runnersList;
    public List<GameObject> rollerSkatersList;
    public List<GameObject> skaterboardersList;

    public GameObject newRunnerButton;
    public GameObject mergeButton;
    public GameObject newObstacleButton;
    public GameObject addPathButton;

    public GameObject rollerSkaterCourse;
    public GameObject skateboarderCourse;

    int currentlyActiveObstacles = 1;
    public GameObject[] obstacles;

    public Transform spawnLocation;

    public Text runnerPriceText;
    public Text mergePriceText;
    public Text obstaclePriceText;
    public Text addPathPriceText;

    float baseSpeed;
    public int money = 0;
    public int runnerPrice = 100;
    public int mergePrice = 1000;
    public int obstaclePrice = 500;
    public int addPathPrice = 2000;

    public bool runnerMaxedOut = false;
    public bool mergeMaxedOut = false;
    public bool obstacleMaxedOut = false;
    public bool addPathMaxedOut = false;

    public bool timeSpedUp = false;

    public bool runFaster = false;
    public bool spedUp = false;
    readonly float tapTimerMax = 3f;
    float tapTimerCurrent = 0f;

    public Text moneyText;

    public CinemachineVirtualCamera runnerPathCam;
    public CinemachineVirtualCamera rollerSkaterPathCam;
    public CinemachineVirtualCamera skateboarderPathCam;

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

        if (Input.GetButtonDown("Jump")){
            money += 1000;
            UpdateMoneyText();
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

        if (money >= runnerPrice && runnersList.Count < 8) {
            newRunnerButton.GetComponent<Button>().interactable = true;
        } else {
            newRunnerButton.GetComponent<Button>().interactable = false;
        }

        if (money >= mergePrice && CheckMerge()) {
            mergeButton.GetComponent<Button>().interactable = true;
        } else {
            mergeButton.GetComponent<Button>().interactable = false;
        }

        if(money >= obstaclePrice && CheckObstacles()) {
            newObstacleButton.GetComponent<Button>().interactable = true;
        } else {
            newObstacleButton.GetComponent<Button>().interactable = false;
        }

        if(money >= addPathPrice && !addPathMaxedOut) {
            addPathButton.GetComponent<Button>().interactable = true;
        } else {
            addPathButton.GetComponent<Button>().interactable = false;
        }
    }

    public void SpawnNewRunner() {
        GameObject newRunner = Instantiate(runnerPrefab, spawnLocation);
        if (runFaster) {
            newRunner.GetComponent<PathCreation.Examples.PathFollower>().speed = baseSpeed * 2f;
        }
        newRunner.GetComponent<PathCreation.Examples.PathFollower>().pathCreator = runnerPath;
        if (runnersList.Count == 0) {
            newRunner.GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled = 0;
        } else {
            newRunner.GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled =
            runnersList[0].GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled - NextRunnerPosition(runnersList, runnerPath);
        }
        
        //Debug.Log(NextRunnerPosition(runnersList, runnerPath));
        runnersList.Add(newRunner);
        money -= runnerPrice;
        runnerPrice = (int)(runnerPrice * 1.15f);
        UpdateMoneyText();
        CheckRunners();

    }

    public float NextRunnerPosition(List<GameObject> athletesList, PathCreator thePath) {
        return athletesList.Count switch {
            1 => thePath.path.length / 2,
            2 => thePath.path.length / 4,
            3 => thePath.path.length * 3 / 4,
            4 => thePath.path.length / 8,
            5 => thePath.path.length * 5 / 8,
            6 => thePath.path.length * 3 / 8,
            7 => thePath.path.length * 7 / 8,
            _ => 0,
        };
    }

    public void AddPath() {
        if (runnerPathCam.Priority > rollerSkaterPathCam.Priority) {
            runnerPathCam.Priority = 10;
            rollerSkaterPathCam.Priority = 20;
            rollerSkaterCourse.SetActive(true);
            if (!obstacles[4].activeInHierarchy) {
                obstacles[4].SetActive(true);
            }
            money -= addPathPrice;
            addPathPrice *= 3;
            UpdateMoneyText();
        } else if (rollerSkaterPathCam.Priority > skateboarderPathCam.Priority) {
            rollerSkaterPathCam.Priority = 10;
            skateboarderPathCam.Priority = 20;
            skateboarderCourse.SetActive(true);
            if (!obstacles[8].activeInHierarchy) {
                obstacles[8].SetActive(true);
            }
            money -= addPathPrice;
            addPathMaxedOut = true;
            UpdateMoneyText();

        }
    }

    public void MergeRunners() {
        if(runnersList.Count >= 3) {
            SpawnNewRollerskater();
        } else {
            SpawnNewSkateboarder();
        }
        

        money -= mergePrice;
        mergePrice = (int)(mergePrice * 1.15f);
        UpdateMoneyText();
        CheckRunners();

    }

    /*public float NextSkaterPosition() {
        return rollerSkatersList.Count switch {
            1 => rollerSkaterPath.path.length / 2,
            2 => rollerSkaterPath.path.length / 4,
            3 => rollerSkaterPath.path.length * 3 / 4,
            4 => rollerSkaterPath.path.length / 8,
            5 => rollerSkaterPath.path.length * 5 / 8,
            6 => rollerSkaterPath.path.length * 3 / 8,
            7 => rollerSkaterPath.path.length * 7 / 8,
            _ => 0,
        };
    }*/

    public void SpawnNewRollerskater() {
        for (int i = 0; i < 3; i++) {
            Destroy(runnersList[runnersList.Count - 1]);
            runnersList.RemoveAt(runnersList.Count - 1);
        }
        GameObject newRollerSkater = Instantiate(rollerSkaterPrefab, spawnLocation);
        newRollerSkater.GetComponent<PathCreation.Examples.PathFollower>().pathCreator = rollerSkaterPath;
        if (rollerSkatersList.Count == 0) {
            newRollerSkater.GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled = 0;
        } else {
            newRollerSkater.GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled
            = rollerSkatersList[0].GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled - NextRunnerPosition(rollerSkatersList, rollerSkaterPath);
        }
        rollerSkatersList.Add(newRollerSkater);
    }

    public void SpawnNewSkateboarder() {
        for (int i = 0; i < 3; i++) {
            Destroy(rollerSkatersList[rollerSkatersList.Count - 1]);
            rollerSkatersList.RemoveAt(rollerSkatersList.Count - 1);
        }
        GameObject newSkateboarder = Instantiate(skateboarderPrefab, spawnLocation);
        newSkateboarder.GetComponent<PathCreation.Examples.PathFollower>().pathCreator = skateboarderPath;
        if (skaterboardersList.Count == 0) {
            newSkateboarder.GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled = 0;
        } else {
            newSkateboarder.GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled
            = skaterboardersList[0].GetComponent<PathCreation.Examples.PathFollower>().distanceTravelled - NextRunnerPosition(skaterboardersList, skateboarderPath);
        }
        skaterboardersList.Add(newSkateboarder);
    }

    public void UpdateMoneyText() {
        moneyText.text = money.ToString();

        if (runnerMaxedOut) {
            runnerPriceText.text = "MAX";
        } else {
            runnerPriceText.text = runnerPrice.ToString();
        }

        if (mergeMaxedOut) {
            mergePriceText.text = "MAX";
        } else {
            mergePriceText.text = mergePrice.ToString();
        }

        if (obstacleMaxedOut) {
            obstaclePriceText.text = "MAX";
        } else {
            obstaclePriceText.text = obstaclePrice.ToString();
        }

        if (addPathMaxedOut) {
            addPathPriceText.text = "MAX";
        } else {
            addPathPriceText.text = addPathPrice.ToString();
        }
    }

    void SpeedUpEveryone() {
        foreach (GameObject runner in runnersList) {
            runner.GetComponent<PathCreation.Examples.PathFollower>().speed = baseSpeed * 2f;
            runner.GetComponentInChildren<Animator>().Play("FastRun");
        }
        foreach (GameObject skater in rollerSkatersList) {
            skater.GetComponent<PathCreation.Examples.PathFollower>().speed = baseSpeed * 2f;
        }
    }
    void ResetSpeed() {
        foreach (GameObject runner in runnersList) {
            runner.GetComponent<PathCreation.Examples.PathFollower>().speed = baseSpeed;
            runner.GetComponentInChildren<Animator>().Play("Run");
        }
        foreach (GameObject skater in rollerSkatersList) {
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

        if(currentlyActiveObstacles == obstacles.Length) {
            obstacleMaxedOut = true;
        }

        money -= obstaclePrice;
        obstaclePrice = (int)(obstaclePrice * 1.15f);
        UpdateMoneyText();
    }

    public bool CheckObstacles() {
        if(!rollerSkaterCourse.activeInHierarchy && currentlyActiveObstacles < 4) {
            return true;
        } else if(rollerSkaterCourse.activeInHierarchy && !skateboarderCourse.activeInHierarchy && currentlyActiveObstacles < 8) {
            return true;
        } else if(skateboarderCourse.activeInHierarchy && currentlyActiveObstacles < 12) {
            return true;
        } else {
            return false;
        }
    }

    public bool CheckMerge() {
        if(runnersList.Count >= 3 && rollerSkatersList.Count < 8 && rollerSkaterCourse.activeInHierarchy) {
            return true;
        } else if (rollerSkatersList.Count >= 3 && skaterboardersList.Count < 8 && skateboarderCourse.activeInHierarchy) {
            return true;
        } else {
            return false;
        }
    }

    public void CheckRunners() {
        if(skaterboardersList.Count == 8 && rollerSkatersList.Count == 8) {
            mergeMaxedOut = true;
            if(runnersList.Count == 8) {
                runnerMaxedOut = true;
            }
            UpdateMoneyText();
        }
    }
}
