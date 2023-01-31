using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour {

    public string sceneName;

    private GameManager manager;
    private Button myButton;

    void Awake()
    {
        manager = GameObject.FindObjectOfType<GameManager>();
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        manager.LoadLevel(sceneName);
    }
}
