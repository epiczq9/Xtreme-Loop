using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        other.gameObject.GetComponent<RunnerBehaviour>().Jump();
        /*switch (other.gameObject.tag) {
            case "Runner":
                other.gameObject.GetComponent<RunnerBehaviour>().Jump();
                break;

            case "Skater":
                break;
        }*/
    }
}
