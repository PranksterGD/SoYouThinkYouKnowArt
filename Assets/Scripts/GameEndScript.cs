using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameEndScript : MonoBehaviour
{
 [SerializeField] public Canvas endGameMenu;
 [SerializeField] public Button playAgain;
 [SerializeField] public Button levelSelect;
 [SerializeField] public Button mainMenu;  
 [SerializeField] public Text score;
    public Text loseText;

	// Use this for initialization
	void Start ()
    {
        endGameMenu = endGameMenu.GetComponent<Canvas>();
        playAgain = playAgain.GetComponent<Button>();
        levelSelect = levelSelect.GetComponent<Button>();
        mainMenu = mainMenu.GetComponent<Button>();
        score = score.GetComponent<Text>();
        loseText = loseText.GetComponent<Text>();
        loseText.enabled = false;
        updateScore();
    }

    public void updateScore()
    {
        score.text = GameManager.score.ToString();

        if(GameManager.playerLose==true)
        {
            loseText.enabled = true;
        }
    }

    public void playAgainPressed()
    {
        SceneManager.LoadScene(1);
    }
    
    public void mainMenuPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void levelSelectPressed()
    {
        MenuScript.gotoLevelSelect = true;
        SceneManager.LoadScene(0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
