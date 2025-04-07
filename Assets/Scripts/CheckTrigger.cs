using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTrigger : MonoBehaviour
{
   
    public bool hit;
    public static CheckTrigger Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        
    }

    public void OnTriggerEnter(Collider other)
    {     
            if (other.gameObject.CompareTag("Platform Trigger"))
            { hit = true;
            Debug.Log("Hit detected");
        
        }
   
    }

    public void OnTriggerExit(Collider other)
    {
            if (other.gameObject.CompareTag("Platform Trigger"))
            { hit = false;
            Debug.Log("Hit Disabled");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
