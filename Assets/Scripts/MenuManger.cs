using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //see the buttons in the editor for how to connect a button click to a function
    public void Quit()
    {
        //thanks for playing
        Application.Quit();
    }

    public void Play()
    {
        //can load scene by scene name (name of .unity file) or load index (order it appears in the scene list when you build the game)
        SceneManager.LoadScene(1);
    }
}
