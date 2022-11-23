using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timers;

public class FloatingText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        TimersManager.SetTimer(this, 0.5f, DestroyText);
    }

    private void LateUpdate() {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }

    void DestroyText() {
        Destroy(gameObject);
    }

}
