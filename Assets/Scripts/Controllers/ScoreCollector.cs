using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCollector : MonoBehaviour
{
    public GameObject waterSplashPrefab;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "ScoreBox")
        {
            Destroy(collision.collider.gameObject);
            
            SoundManager.instance.PlaySplashOneShot();

            Instantiate(waterSplashPrefab,collision.contacts[0].point+Vector3.up*2,Quaternion.identity);
            StageController.instance.AddScore();
        }
    }
}
