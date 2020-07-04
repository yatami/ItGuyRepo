using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPatternController : MonoBehaviour
{
    GameObject camera;
    LineRenderer lineRenderer;
    GameObject gameController;

    bool[,] nodeCheck;
    int[] pattern;
    List<int[]> correctPatterns;

    public float maxFailCount = 3;
    float failCount;
    float lineY;
    int patternSize;
    int numberOfPatterns;
    int numberOfSolvedPatterns = -1;
    bool nodeClicked;
    bool isThereAnyCorrectPattern;



    // Start is called before the first frame update
    void Start()
    {
        

    }

    private void Awake()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        gameController = GameObject.FindGameObjectWithTag("GameControl");
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        nodeCheck = new bool[3, 3];
        pattern = new int[9];
        StartCoroutine(getNumberOfPattersAfterDelay());
        Debug.Log("awake Called");
    }

    // Update is called once per frame
    void Update()
    {
        if(failCount >= maxFailCount)
        {
            gameController.GetComponent<GameController>().levelFailed();
        }

        if(numberOfSolvedPatterns >= numberOfPatterns)
        {
            gameController.GetComponent<GameController>().levelWon();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            checkClickPosition();
        }

        if (Input.GetMouseButton(0) && nodeClicked)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                lineRenderer.SetPosition(lineRenderer.positionCount-1, new Vector3(hit.point.x, lineY, hit.point.z));
                if (hit.transform.CompareTag("Node"))
                {
                    int posX = hit.transform.gameObject.GetComponent<NodeNumber>().nodePosX;
                    int posY = hit.transform.gameObject.GetComponent<NodeNumber>().nodePosY;
                    if (nodeCheck[posX, posY] == false)
                    {
                        pattern[lineRenderer.positionCount - 1] = hit.transform.gameObject.GetComponent<NodeNumber>().nodeNum;
                        nodeCheck[posX, posY] = true;
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 2, new Vector3(hit.transform.position.x, lineY, hit.transform.position.z));
                    }
                }
            }
        }

        if(Input.GetMouseButtonUp(0) && nodeClicked)
        {
            patternSize = lineRenderer.positionCount - 1;
            compareWithAllPatterns();
            StartCoroutine(WaitAndReset());
        }
    }

    private void compareWithAllPatterns()
    {
        isThereAnyCorrectPattern = false;
        foreach (int[] pat in correctPatterns)
        {
            bool identical = comparePatterns(pat);
            if (identical)
            {
                lineRenderer.SetColors(Color.green, Color.green);
                correctPatterns.Remove(pat);
                numberOfSolvedPatterns++;
                isThereAnyCorrectPattern = true;
                break;
            }
            else
            {
                lineRenderer.SetColors(Color.red, Color.red);
            }
        }
        if(!isThereAnyCorrectPattern)
        {
            failCount++;
        }
    }

    // read patterns and compare both reverse and linear
    private bool comparePatterns(int[] pat)
    {
        bool mismatch = false;
        if (pat.Length != patternSize)
        {
            return false;
        }

  
       for(int i = 0; i < pat.Length; i++)
        {
            if(pat[i] != pattern[i])
            {
                mismatch = true;
            }
        }

       if(!mismatch)
        {
            return true;
        }
        mismatch = false;
        int index = 0;
        for (int i = pat.Length-1; i >= 0; i--)
        {
            if (pat[i] != pattern[index])
            {
                mismatch = true;
            }
            index++;
        }
        if (!mismatch)
        {
            return true;
        }

        return false;
    }

    private void printAndResetPattern()
    {
        lineRenderer.SetColors(Color.white, Color.white);
        /*for (int i = 0; i < 3; i++)
        {
            Debug.Log(nodeCheck[0, i] + " " + nodeCheck[1, i] + " " + nodeCheck[2, i]);
        }*/
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                nodeCheck[i, j] = false;
            }
        }
        for (int i = 0; i < patternSize; i++)
        {
            Debug.Log(pattern[i]);
        }
    }

    private void checkClickPosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.DrawLine(camera.transform.position, hit.point, Color.white, 2.5f);
            if (hit.transform.CompareTag("Node"))
            {
                lineRenderer.SetPosition(0, hit.transform.position);
                pattern[0] = hit.transform.gameObject.GetComponent<NodeNumber>().nodeNum;
                int posX = hit.transform.gameObject.GetComponent<NodeNumber>().nodePosX;
                int posY = hit.transform.gameObject.GetComponent<NodeNumber>().nodePosY;
                nodeCheck[posX, posY] = true;
                lineY = hit.transform.position.y;
                nodeClicked = true;
            }
        }
    }


    public void addCorrectPattern(int[] pattern)
    {
        if (correctPatterns == null)
        {
            correctPatterns = new List<int[]>();
            Debug.Log("correct pattern initilized");
        }
        Debug.Log("correct pattern added");
        correctPatterns.Add(pattern);
    }

    IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(1f);
        printAndResetPattern();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
        yield break;
    }

    IEnumerator getNumberOfPattersAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("number of patters = " + correctPatterns.Count);
        numberOfPatterns = correctPatterns.Count;
        numberOfSolvedPatterns = 0;
        yield break;
    }
}
