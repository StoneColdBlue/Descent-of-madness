using UnityEngine;

public class Knowledge : MonoBehaviour
{
    public static int coinTally = 0;
    [SerializeField] GameObject coinDisplay;

    // Update is called once per frame
    void Update()
    {
        coinDisplay.GetComponent<TMPro.TMP_Text>().text = "COINS: " + coinTally;
    }
}
