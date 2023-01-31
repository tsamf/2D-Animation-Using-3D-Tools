using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour {

    private GameManager manager;
    private Button myButton;

	void Awake () {
        manager = GameObject.FindObjectOfType<GameManager>();
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(TaskOnClick);
	}
	
	void TaskOnClick () {
       manager.LoadNextLevel();
	}
}
