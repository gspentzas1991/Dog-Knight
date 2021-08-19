using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGroundCollision : MonoBehaviour
{
    private Fighter parentController;

    void Start()
    {
        parentController = transform.parent.gameObject.GetComponent<Fighter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            parentController.TouchedGround();
        }
    }
}
