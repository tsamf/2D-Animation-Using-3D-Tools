using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    public int ID;
    public AudioClip deathSound;
    public int points = 0;
    public bool isEnemy = false;

    private LevelManager levelmanager;
    private bool isActive = false;
    private Animator animator;
    private bool dying = false;

    private float timeAlive;

    private CSVWriter writer;

    private void Awake()
    {
        levelmanager = GameObject.FindObjectOfType<LevelManager>();
        writer = GameObject.FindObjectOfType<CSVWriter>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {

        if(isActive)
        {
            timeAlive += Time.deltaTime;
        }

        //If the NPC is hit start Dying Coroutine
        if (dying)
        {
            StartCoroutine(Dying());
        }
    }

    protected IEnumerator Dying()
    {
        //Set dying false so the Coroutine is only called once
        dying = false;

        //Set animation state to dying
        animator.SetBool("Death_b", true);

        //Inform the Levelmanager that an NPC has been hit 
        int totalPoints = levelmanager.NPCHit(points, isEnemy);

        //Play dying Sound
        AudioSource.PlayClipAtPoint(deathSound, GameObject.FindObjectOfType<Camera>().transform.position, .5f);

        //write Death to tracker
        if(isEnemy)
        {
            writer.GetComponent<CSVWriter>().AddEnemy(new EnemyData(ID, totalPoints, timeAlive,gameObject.name));
        }
        else
        {        
            writer.GetComponent<CSVWriter>().AddFriendly(new FriendlyData(ID,timeAlive,gameObject.name));
        }


        //Wait 5 seconds and then destroy the NPC
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    public void SetActive()
    {
        animator.SetFloat("Speed_f", 5f);
        isActive = true;
    }

    private void OnMouseDown()
    {
        //If the NPC is clicked it is now dying and set to inactive so it can't be shot again
        if (isActive)
        { 
            dying = true;
            isActive = false;
        }
    }
}
