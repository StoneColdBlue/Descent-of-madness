using UnityEngine;

public class PlayerMovment : MonoBehaviour
{

    public float playerSpeed = 10;
    public float horizontalSpeed = 10;
    public float rightLimit = 6;
    public float leftLimit = -6;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward *Time.deltaTime * playerSpeed);



        if (Input.GetKey(KeyCode.A))
        {
            if (this.gameObject.transform.position.z > leftLimit)
            {
                transform.Translate(Vector3.left * Time.deltaTime * horizontalSpeed);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (this.gameObject.transform.position.z < rightLimit)
            {
                transform.Translate(Vector3.right * Time.deltaTime * horizontalSpeed);
            }
        }
    }
}
