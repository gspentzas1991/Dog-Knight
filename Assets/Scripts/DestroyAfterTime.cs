using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float timer=2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyOnTimer(timer));
    }

    IEnumerator DestroyOnTimer(float destructionTimer)
    {
        yield return new WaitForSeconds(destructionTimer);
        Destroy(gameObject);
    }
}
