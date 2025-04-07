using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int rotateSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);
    }
}
