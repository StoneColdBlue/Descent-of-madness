using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Knowledge.coinTally += 1;
        this.gameObject.SetActive(false);
    }
}
