using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys any fighter or powerup that touches it
/// </summary>
public class DestroyCollider : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")
            || collision.gameObject.CompareTag("Powerup"))
        {
            if (collision.gameObject.TryGetComponent(out Fighter fighter))
            {
                fighter.Destroy();
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
