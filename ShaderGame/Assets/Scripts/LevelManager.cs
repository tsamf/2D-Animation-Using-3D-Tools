using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;




public class LevelManager : MonoBehaviour {

    public float levelTime = 180;
    public float multiplierTime = 5;
    public int seed = 1;
    public int enemyCount = 25;
    public int friendlyCount = 75;
    public AudioClip rifleSound;
    public GameObject[] enemyObjects;
    public GameObject[] friendlyObjects;

    private Text timerText;
    private Text scoreText;
    private Text multiplierText;
    private int score = 0;
    private int multiplier = 1;
    private float multiplierTimeLeft;
    private int npcID = 1;
    private int releaseID = 1;
    private float timePassed;
    private float timePerActivate;
    private float timeSinceLastActive;
    private  System.Random rand;
    private List<int> randomizeNPCOrder = new List<int>();
    private GameManager gameManager;
    private Dictionary<int,GameObject> npcObjects = new Dictionary<int, GameObject>();
    private CSVWriter writer;
    private string currentShader;

    private int clickCount = 0;
    private int enemiesKilled = 0;
    private int friendliesKilled = 0;
    private int highestMultiplier = 1;

    private void Awake()
    {
        //Create Random with seed
        rand = new System.Random(seed);

        //Find the GameManger in the scene
        gameManager = GameObject.FindObjectOfType<GameManager>();

        //Find CSV writer
        writer = GameObject.FindObjectOfType<CSVWriter>();

        //Grab UI text objects to update
        timerText = GameObject.Find("Timer").GetComponent<Text>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        multiplierText = GameObject.Find("Multiplier").GetComponent<Text>();
        UpdateText();

        //Generate people
        SpawnPeople();

        //Calculate the speed that the NPCS need to be released
        timePerActivate = (levelTime - 10) / (enemyCount + friendlyCount);

        //Create Random list for activating the npcs
        PopulateRandomIDOrder();
    }

	// Update is called once per frame
	void Update () {
        
        //Update Time
        timeSinceLastActive += Time.deltaTime;
        timePassed += Time.deltaTime;

        //End level when time runs out
        if(levelTime - timePassed <= 0)
        {
            SendAnalytics();
   
            GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
            UpdateResultsCanvas();
            GameObject.Find("ResultCanvas").GetComponent<Canvas>().enabled = true;

            

            //gameManager.LoadNextLevel();
        }

        UpdateNPCs();

        HandleInput();

        UpdateMultiplier();

        UpdateText();

       
	}

    private void UpdateResultsCanvas()
    {    
        GameObject.Find("Prisoners").GetComponent<Text>().text = "Prisoners : " + enemiesKilled.ToString();
        GameObject.Find("Civilians").GetComponent<Text>().text = "Civilians : " + friendliesKilled.ToString();
        GameObject.Find("MaxMultiplier").GetComponent<Text>().text = "Multiplier : " + highestMultiplier.ToString();
        GameObject.Find("FinalScore").GetComponent<Text>().text = "Score : " + score.ToString(); 
    }

    //Activates NPCs as time passes
    private void UpdateNPCs()
    {
        //Realse the walkers
        if (timeSinceLastActive > timePerActivate && releaseID <= (enemyCount + friendlyCount))
        {
            //release
            npcObjects[randomizeNPCOrder[releaseID - 1]].GetComponent<NPC>().SetActive();

            //increment realeasID
            releaseID++;

            timeSinceLastActive -= timePerActivate;
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioSource.PlayClipAtPoint(rifleSound, GameObject.FindObjectOfType<Camera>().transform.position, 1);
            clickCount++;
        }
    }

    private void UpdateMultiplier()
    {
        multiplierTimeLeft -= Time.deltaTime;
        // Reset multiplier if to much time passes   
        if (multiplierTimeLeft <= 0)
            multiplier = 1;
    } 

    private void SendAnalytics()
    {
        //Send Info to CSV writer
        TotalData total = new TotalData(
               score,
               highestMultiplier,
               enemiesKilled,
               friendliesKilled,
               clickCount,
               gameManager.GetComponent<GameManager>().GetCurrentLevel(),
               SceneManager.GetActiveScene().name,
                currentShader
            );

        writer.GetComponent<CSVWriter>().SetTotal(total);

        //Send Info to Unity Analytics
        Analytics.CustomEvent("levelComplete", new Dictionary<string, object>
            {
                {"clicks",clickCount},
                {"enemies", enemiesKilled},
                {"Friendlies",friendliesKilled},
                {"Score",score},
                {"Highest Multiplier", highestMultiplier },
                {"Level Order", gameManager.GetComponent<GameManager>().GetCurrentLevel()},
                {"Actual Level", SceneManager.GetActiveScene().name},
                {"Shader Setting", currentShader }
            });
    }

    //Update UI Text 
    void UpdateText()
    {
        float timeLeft = levelTime - timePassed;

        int mintues = (Int32)(timeLeft / 60);
        int seconds = (Int32)(timeLeft % 60);
        int secs = (Int32)(timeLeft);
        int miliseconds = (Int32)((timeLeft - secs) * 100);

        timerText.text = string.Format("Time: {0}:{1}:{2}",mintues,seconds,miliseconds);
        scoreText.text = string.Format("Score: {0}", score.ToString());
        multiplierText.text = string.Format("X{0}", multiplier.ToString());
    }

    //NPC is shot Update Multiplier,Score, and Kill count
    public int NPCHit(int points, bool isEnemy)
    {
        int totalPoints = 0;

        if (isEnemy)
        {
            totalPoints = points * multiplier;
            score += totalPoints;
            multiplier++;

            //If highest Multiplier update
            if (multiplier > highestMultiplier)
            {
                highestMultiplier = multiplier;
            }
             
            multiplierTimeLeft = multiplierTime;
            enemiesKilled++;
        }
        else
        {
            multiplier = 1;
            multiplierTimeLeft = 0;
            friendliesKilled++;
        }

        //Update UI text to reflect new score and multiplier
        UpdateText();

        return totalPoints;
    }

    public void SpawnPeople()
    {
        //Get Correct Enemy shader for level
      

        if (gameManager != null)
        {
            currentShader = gameManager.GetComponent<GameManager>().GetCurrentShader();
        }
       
        //Spawn enemies
        switch(currentShader)
        {
            case "None":
                spawnNPCs(new GameObject[] {enemyObjects[0]}, enemyCount);
                break;
            case "Outline":
                spawnNPCs(new GameObject[] { enemyObjects[1]}, enemyCount);
                break;
            case "Shadow":
                spawnNPCs(new GameObject[] { enemyObjects[2]}, enemyCount);
                break;
            case "Both":
                spawnNPCs(new GameObject[] { enemyObjects[3]} , enemyCount);
                break;
            default:
                spawnNPCs(new GameObject[] { enemyObjects[0] }, enemyCount);
                break;
        }

        //spawn friendlies
        spawnNPCs(friendlyObjects, friendlyCount);

    }

    private int GetNextID()
    {
        return npcID++;
    }

    //Used to populate a randomized list of the npcs IDs for activation
    private void PopulateRandomIDOrder()
    {
        do
        {
            int num =rand.Next(1, (friendlyCount + enemyCount)+1);

            if(!randomizeNPCOrder.Contains(num))
            {
                randomizeNPCOrder.Add(num);
            }

        } while (randomizeNPCOrder.Count < (friendlyCount + enemyCount));

    }

    //Populates the NPCs in the scene
    public void spawnNPCs(GameObject[] prefabs,int count)
    {
        while (count > 0)
        {
            //Generate position on alternating sides
            Vector3 pos = new Vector3();
            int j = count % 4;
            switch (j)
            {
                case 3:
                    pos = new Vector3(rand.Next(-25, 25), 0, -25);
                    break;
                case 2:
                    pos = new Vector3(-25, 0, rand.Next(-25, 25));
                    break;
                case 1:
                    pos = new Vector3(rand.Next(-25, 25), 0, 25);
                    break;
                case 0:
                    pos = new Vector3(25, 0, rand.Next(-25, 25));
                    break;
            }

            //Grab a random NPC prefab from the prefabs array
            int index = rand.Next(0, prefabs.Length);

            //Spawn the object using the prefab 
            GameObject spawnedObject = (GameObject)Instantiate(prefabs[index], pos, Quaternion.identity, gameObject.transform);

            //Grab the next ID
            int id = GetNextID();
           
            //Set the objects ID
            spawnedObject.GetComponent<NPC>().ID = id;

            spawnedObject.name = spawnedObject.name +id;
            
            //Get angle for rotation to origin
            float angle = Vector3.Angle(Vector3.forward, pos);

            //Offset angle so all NPCs arent walking through center of the play area
            float angleOffset = rand.Next(-30, 30);

            //Rotate NPC to walk through the scene
            if (pos.x > 0)
            {
                spawnedObject.transform.Rotate(0, angle + 180 + angleOffset, 0);
            }
            else
            {
                spawnedObject.transform.Rotate(0, 180 - angle + angleOffset, 0);
            }

            //Add Object to the internal dictionary for later reference
            npcObjects.Add(id, spawnedObject);

            //reduce count
            count--;
        }
    }
}
