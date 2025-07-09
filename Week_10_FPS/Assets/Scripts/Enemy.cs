using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Rigidbody ragdollRB in GetComponentsInChildren<Rigidbody>()) 
        {
            ragdollRB.isKinematic = true;
        }
    }

  public void triggerRagdoll()
    {
        foreach (Rigidbody ragdollRB in GetComponentsInChildren<Rigidbody>()) 
        {
            ragdollRB.isKinematic = false;
        }
        GetComponent<Collider>().enabled = false;
    }
}
