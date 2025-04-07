using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentGenerator : MonoBehaviour
{
    //public GameObject[] segment;
    public List<GameObject> segments;
    public CheckTrigger trigger;
    public DestroyPlatforms canDestroy;
    private int lastIndex = -1;

    [SerializeField] int xPos = -85;
    [SerializeField] int yPos = -2;

    [SerializeField] int xDiff = -80;
    [SerializeField] int yDiff = -5;
   // [SerializeField] bool spawnSegment = false;
    [SerializeField] int segmentNum;

    private bool hasSpawned = false; // Track if we've already spawned this trigger
    private List<GameObject> spawnedSegments = new List<GameObject>();

    void Update()
    {
        if (CheckTrigger.Instance != null && CheckTrigger.Instance.hit && !hasSpawned)
        {
            if (segments.Count == 0) return;

            // Get a random index different from the last one
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, segments.Count);
            } while (randomIndex == lastIndex && segments.Count > 1);

            // Instantiate the new prefab
            //Instantiate(segments[randomIndex], new Vector3(xPos, 0, 0), Quaternion.identity);
            //xPos += -70;
            
            GameObject newSegment = Instantiate(segments[randomIndex], new Vector3(xPos, yPos, 0), Quaternion.identity);
            xPos += xDiff;
            yPos += yDiff;
            spawnedSegments.Add(newSegment);
            lastIndex = randomIndex;

            Destroy(newSegment, 16f);

            hasSpawned = true; // Mark that we've spawned for this trigger

            Debug.Log("Spawned Platform");

   
        }
        else if (CheckTrigger.Instance != null && !CheckTrigger.Instance.hit)
        {
            hasSpawned = false; // Reset when trigger is no longer active
        }

        void LateUpdate()
        {
            // Remove null entries (destroyed segments) from the list
            spawnedSegments.RemoveAll(item => item == null);
        }


        //GetComponent<CheckTrigger>() = trigger;


    }
/*
    IEnumerator SegmentGen()
    {
        segmentNum = Random.Range(0, 3);
        Instantiate(segment[segmentNum], new Vector3(xPos, 0, 0), Quaternion.identity);
        xPos += -50;
        yield return new WaitForSeconds(3);
        spawnSegment = false;
        Destroy(SegmentGen, 3f);

    }
*/
    private void Destroy(System.Func<IEnumerator> segmentGen, float v)
    {
        throw new System.NotImplementedException();
    }
}
