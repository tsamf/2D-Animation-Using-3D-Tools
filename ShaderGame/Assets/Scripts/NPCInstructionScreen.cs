using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInstructionScreen : MonoBehaviour {

    public AudioClip deathSound;
    public Image underImage;
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        //Show the UI image under the NPC 
        underImage.enabled = true;
        
        //Play dying Sound
        AudioSource.PlayClipAtPoint(deathSound, GameObject.FindObjectOfType<Camera>().transform.position, .5f);

        //Play dying animation
        animator.SetBool("Death_b", true);
    }
}
