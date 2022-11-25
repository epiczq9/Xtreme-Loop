using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpawn : MonoBehaviour
{
    public GameObject SpawnPointUI(GameObject points) {
        return Instantiate(points, transform);
    }
}
