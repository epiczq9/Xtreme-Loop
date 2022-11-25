using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Skateboarder")) {
            other.gameObject.GetComponent<SkaterBehaviour>().Jump();
        } else {
            other.gameObject.GetComponent<RunnerBehaviour>().Jump();
        }
        /*switch (other.gameObject.tag) {
            case "Runner":
                other.gameObject.GetComponent<RunnerBehaviour>().Jump();
                break;

            case "Skater":
                break;
        }*/
    }
}
