using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBox : MonoBehaviour
{
    public LayerMask layer;

    public GameObject explosionParticle;
    private void Start()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Man")
        {
                SoundManager.instance.PlayHitOneShot();
        }
    }
}
