using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    private static MusicPlayer instance = null;

    private void Awake()
    {
        //If a music player already exists destroy this one
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {   
            //Add this instance if the singleton is null
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}
