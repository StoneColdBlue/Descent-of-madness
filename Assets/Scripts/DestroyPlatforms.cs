using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlatforms : MonoBehaviour
{
    public bool destroyDatBitch;
    public static DestroyPlatforms Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destroy Trigger"))
        {
            destroyDatBitch = true;
            Debug.Log("Bitch being destroyed");

        }
    }
        // Update is called once per frame
        void Update()
    {
        
    }
}
