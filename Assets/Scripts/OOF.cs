using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OOF : MonoBehaviour
{
    [SerializeField] GameObject Player1;
    [SerializeField] GameObject CameraMain;
    [SerializeField] GameObject Canvas;

    void OnTriggerEnter(Collider other)
    {
        Player1 = GameObject.FindWithTag("Player");
        CameraMain = GameObject.FindWithTag("MainCamera");
        Canvas = GameObject.FindWithTag("Canvas");
        StartCoroutine(CollisionEnd());
    }

    IEnumerator CollisionEnd()
    {
        Player1.GetComponent<PlayerMovment>().enabled = false;
        CameraMain.GetComponent<Animator>().Play("Hit The wall Cam");
        yield return new WaitForSeconds(3);
        Canvas.GetComponent<CanvasManager>().TurnOn();
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}
