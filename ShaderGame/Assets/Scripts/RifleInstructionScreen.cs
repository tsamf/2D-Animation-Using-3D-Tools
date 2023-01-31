using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleInstructionScreen : MonoBehaviour {

    public AudioClip rifleSound;

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private void Awake()
    {
        //Set HotSpot to middle of cursorTexture
        hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);

        //Change cursor to new cursorTexture
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }


    void Update () {

        //Play rifleSound on click
		if(Input.GetMouseButtonDown(0))
        {
            AudioSource.PlayClipAtPoint(rifleSound, GameObject.FindObjectOfType<Camera>().transform.position, .5f);
        }
	}
}
