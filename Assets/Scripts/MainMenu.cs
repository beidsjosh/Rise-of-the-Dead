using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

    public void LoadLevel (string levelName) {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame() {
        Application.Quit ();
    }
}