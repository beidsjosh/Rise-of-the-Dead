using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

    public Text scoreText;
    //private int score = 0;
    //protected int scorePlayer = .score;
    //public GameObject player = GameObject.GetComponent<playerScript>();
    //public playerScript PlayerScript;
    //public GameObject PlayerScript;

    /*void Start(){
        //playerScript = GameObject.Find("Player").GetComponent<playerScript>();
        GameObject PlayerScript = gameObject;
        //playerScript player = GameObject.FindObjectOfType(typeof(playerScript)) as Player;
        playerScript player = (playerScript) PlayerScript.GetComponent(typeof(playerScript));
        score = player.score;
        UpdateScore(score);
        //UpdateScore(playerScript.score);
    }*/

    public void LoadLevel (string levelName) {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame() {
        Application.Quit ();
    }

    /*public void UpdateScore(int addScore) {
		// update the GUI score here
		//scoreText.text = "Final Score!!: " + score.ToString();

	}*/
}