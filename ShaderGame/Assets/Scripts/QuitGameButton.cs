using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour {

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
        manager.QuitApplication();
    }
}
