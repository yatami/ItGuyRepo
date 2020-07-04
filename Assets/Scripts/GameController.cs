using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] levels;
    public int levelIndex;
    public GameObject winPanel;
    public GameObject failPanel;

    GameObject currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = Instantiate(levels[levelIndex], new Vector3(0, 0, 0), Quaternion.identity);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void levelWon()
    {
        winPanel.SetActive(true);
    }

    public void levelFailed()
    {
        failPanel.SetActive(true);
        
    }

    public void continueLevel()
    {
       
        Destroy(currentLevel);
        StartCoroutine(continueWithDelay());
    }


    public void tryAgain()
    {
        Destroy(currentLevel);

        StartCoroutine(tryAgainWithDelay());

    }

    IEnumerator tryAgainWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject currentLevelToSet = Instantiate(levels[levelIndex], new Vector3(0, 0, 0), Quaternion.identity);

        currentLevel = currentLevelToSet;
        failPanel.SetActive(false);
        yield break;
    }

    IEnumerator continueWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        levelIndex++;
        GameObject currentLevelToSet = Instantiate(levels[levelIndex], new Vector3(0, 0, 0), Quaternion.identity);

        currentLevel = currentLevelToSet;
        winPanel.SetActive(false);
        yield break;
    }
}
