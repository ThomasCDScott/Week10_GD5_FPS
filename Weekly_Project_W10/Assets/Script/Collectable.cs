using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour
{
     public int value = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            GameManager.instance.AddScore(value);

            
            Destroy(gameObject);
        }
    }
}



