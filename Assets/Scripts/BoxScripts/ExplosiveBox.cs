using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBox : MonoBehaviour
{
    public float power = 3;
    public float radius = 3;
    public float modeif = 3;

    public LayerMask layer;

    public GameObject explosionParticle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Man")
        {
            Collider[] overlap = Physics.OverlapSphere(transform.position, 20, layer);
            foreach(Collider hit in overlap)
            {
                var rigid = hit.GetComponent<Rigidbody>();
                rigid.AddExplosionForce(power, transform.position, radius, modeif, ForceMode.Impulse);
                Instantiate(explosionParticle,transform.position,Quaternion.identity);
            }
                StageController.instance.AddScore();
                SoundManager.instance.PlayExplosionOneShot();
                Destroy(gameObject);
            
        }
    }
}
