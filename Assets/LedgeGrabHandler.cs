using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BehaviourController))]
public class LedgeGrabHandler : MonoBehaviour
{
    /// <summary>
    /// The time after letting go of a ledge, where you can't grab it again
    /// </summary>
    [SerializeField] private float ledgeGrabTimer = 0.5f;
    [SerializeField] public bool isHoldingLedge;
    [SerializeField] private bool canGrabLedge;
    private Fighter fighter;
    private Transform ledgeTransform;
    /// <summary>
    /// How far away from the ledge object the fighter will be staying
    /// </summary>
    [SerializeField] private Vector3 ledgeOffset;

    private void Start()
    {
        fighter = GetComponent<Fighter>();
        canGrabLedge = true;
    }

    // While the fighter is holding a ledge, their transform stays on that ledge. If they receive any input, they let go
    private void Update()
    {
        if (isHoldingLedge && ( fighter._behaviourController.jumpInput.receivedInput))
        {
            ReleaseLedge();
        }
        if (isHoldingLedge)
        {
            transform.position = ledgeTransform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlatformLedge") && canGrabLedge)
        {
            GrabLedge(other.transform);
            fighter.TouchedGround();
        }
    }

    /// <summary>
    /// Updates the isHoldingLedge boolean, and saves the ledge's transform
    /// </summary>
    private void GrabLedge(Transform _ledgeTransform)
    {
        isHoldingLedge = true;
        ledgeTransform = _ledgeTransform;
    }

    /// <summary>
    /// Updates the isHoldingLedge boolean, and starts a timer to disable ledge grabbing
    /// </summary>
    public void ReleaseLedge()
    {
        isHoldingLedge = false;
        canGrabLedge = false;
        StartCoroutine(DisableLedgeGrabing());
    }

    /// <summary>
    /// Makes the gameobject unable to grab a ledge for some seconds
    /// </summary>
    IEnumerator DisableLedgeGrabing()
    {
        canGrabLedge = false;
        yield return new WaitForSeconds(ledgeGrabTimer);
        canGrabLedge = true;
    }
}
