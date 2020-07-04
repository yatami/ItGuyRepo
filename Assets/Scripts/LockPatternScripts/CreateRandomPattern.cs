using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRandomPattern : MonoBehaviour
{
    public GameObject[] nodes;
    public float stepTime = 1f;
    public int patternMinLineNumber = 3;
    public int patternMaxLineNumber = 9;

    LineRenderer lineRenderer;
    GameObject phoneRef;

    int[] pattern;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void Awake()
    {
        phoneRef = GameObject.FindGameObjectWithTag("Phone");
        createPatternRandomly();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        StartCoroutine(drawPatternStepByStep());
        StartCoroutine(getRefWithDelay());
    }

    private void createPatternRandomly()
    {
        // set a random size for pattern
        Random random = new Random();
        int size = Random.Range(3, 9);
        pattern = new int[size];

        // fill the pattern array with random node permutations
        int index = 0;
        while(size - index > 0)
        {
            int randomNode = Random.Range(0, 9);
            if(!checkValue(pattern, randomNode))
            {
                pattern[index] = randomNode;
                index++;
            }
        }

        //print pattern
        for (int i = 0; i < size; i++)
        {
            Debug.Log(pattern[i]);
        }

        // add the pattern to correct patterns list
        phoneRef.GetComponent<LockPatternController>().addCorrectPattern(pattern);
        Debug.Log("phoneref" + phoneRef.gameObject.GetInstanceID());
    }

    IEnumerator drawPatternStepByStep()
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(i, nodes[pattern[i]].transform.position);
            
            yield return new WaitForSeconds(stepTime);
        }
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private bool checkValue(int[] arr, int val)
    {
        for(int i = 0; i< arr.Length; i++)
        {
            if(arr[i] == val)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator getRefWithDelay()
    {
        yield return new WaitForSeconds(0.2f);

        yield break;
    }

}
