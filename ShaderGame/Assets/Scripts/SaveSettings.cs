using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSettings : MonoBehaviour {

    private GameManager manager;
    private Button myButton;

    void Awake()
    {
        //Find the GameManger in the scene
        manager = GameObject.FindObjectOfType<GameManager>();

        //Find the button object this script is attached to and add a listner for clicking on it
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(TaskOnClick);

        //Synch the drop down selection on the UI with the information in the GameManager object
        SyncLevelOrder();
        SyncShaderOrder();
    }

    //Update Dropdowns to reflect Game Managers Level Order
    private void SyncLevelOrder()
    {
        string[] levels = manager.GetLevelOrder();

        GameObject.Find("Level1Dropdown").GetComponent<Dropdown>().value = Convert.ToInt32(levels[0].Substring(levels[0].Length - 1)) - 1;
        GameObject.Find("Level2Dropdown").GetComponent<Dropdown>().value = Convert.ToInt32(levels[1].Substring(levels[1].Length - 1)) - 1;
        GameObject.Find("Level3Dropdown").GetComponent<Dropdown>().value = Convert.ToInt32(levels[2].Substring(levels[2].Length - 1)) - 1;
        GameObject.Find("Level4Dropdown").GetComponent<Dropdown>().value = Convert.ToInt32(levels[3].Substring(levels[3].Length - 1)) - 1;
    }

    //Update DropDowns to reflect Game Managers Shader Order
    private void SyncShaderOrder()
    {
        string[] shaders = manager.GetShaderOrder();

        GameObject.Find("L1ShaderDropdown").GetComponent<Dropdown>().value = GetShaderIndex(shaders[0]);
        GameObject.Find("L2ShaderDropdown").GetComponent<Dropdown>().value = GetShaderIndex(shaders[1]);
        GameObject.Find("L3ShaderDropdown").GetComponent<Dropdown>().value = GetShaderIndex(shaders[2]);
        GameObject.Find("L4ShaderDropdown").GetComponent<Dropdown>().value = GetShaderIndex(shaders[3]);
    }

    //Returns the dropdown index of a shader given its string name
    private int GetShaderIndex(string shader)
    {
        switch(shader)
        {
            case "None":
                    return 0;
            case "Outline":
                    return 1;
            case "Shadow":
                    return 2;
            case "Both":
                    return 3;
            default:
                return 0;
        }
    }

    void TaskOnClick()
    {
        //Update Levels from dropdowns
        string[] levels = new string[4];
        levels[0] = GameObject.Find("Level1Dropdown").GetComponent<Dropdown>().captionText.text;
        levels[1] = GameObject.Find("Level2Dropdown").GetComponent<Dropdown>().captionText.text;
        levels[2] = GameObject.Find("Level3Dropdown").GetComponent<Dropdown>().captionText.text;
        levels[3] = GameObject.Find("Level4Dropdown").GetComponent<Dropdown>().captionText.text;
        manager.SetLevelOrder(levels);

        //Update Shaders from dropdowns
        string[] shaders = new string[4];
        shaders[0] = GameObject.Find("L1ShaderDropdown").GetComponent<Dropdown>().captionText.text;
        shaders[1] = GameObject.Find("L2ShaderDropdown").GetComponent<Dropdown>().captionText.text;
        shaders[2] = GameObject.Find("L3ShaderDropdown").GetComponent<Dropdown>().captionText.text;
        shaders[3] = GameObject.Find("L4ShaderDropdown").GetComponent<Dropdown>().captionText.text;
        manager.SetShaderOrder(shaders);

        //Load the MainMenu Scene
        manager.LoadLevel("MainMenu");
    }
}
