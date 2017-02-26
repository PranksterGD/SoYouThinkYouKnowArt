using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour
{
 [SerializeField]  public Canvas exitMenu;
 [SerializeField] public Button playButton;
 [SerializeField] public Button exitButton;
 [SerializeField]  public Canvas startMenu;
 [SerializeField]  public Canvas levelMenu;
                   public AudioClip menuClickSound;
    public Button instructionsButton;
    public Canvas instructionsMenu;
    static public bool gotoLevelSelect = false;
    public Canvas instructions2Menu;

	// Use this for initialization
	void Start ()
    {
        exitMenu = exitMenu.GetComponent<Canvas>();
        playButton = playButton.GetComponent<Button>();
        exitButton = exitButton.GetComponent<Button>();
        startMenu = startMenu.GetComponent<Canvas>();
        levelMenu = levelMenu.GetComponent<Canvas>();
        instructionsButton = instructionsButton.GetComponent<Button>();
        instructions2Menu = instructions2Menu.GetComponent<Canvas>();
        exitMenu.enabled = false;
        levelMenu.enabled = false;
        instructionsMenu.enabled = false;
        instructions2Menu.enabled = false;
        if(gotoLevelSelect==true)
        {
            PlayPressed();
            gotoLevelSelect = false;
        }
	}

    public void ExitPressed()
    {
        instructionsMenu.GetComponent<AudioSource>().PlayOneShot(menuClickSound);
        exitMenu.enabled = true;
        playButton.enabled = false;
        exitButton.enabled = false;
    }

    public void InstructionsPressed()
    {
        instructionsMenu.GetComponent<AudioSource>().PlayOneShot(menuClickSound);
        startMenu.enabled = true;
        instructionsMenu.enabled = true;
    }

    public void instructionsNextPressed()
    {
        instructionsMenu.GetComponent<AudioSource>().PlayOneShot(menuClickSound);
        instructionsMenu.enabled = false;
        instructions2Menu.enabled = true;

    }


    public void BackPressed()
    {
        instructionsMenu.GetComponent<AudioSource>().PlayOneShot(menuClickSound);
        instructions2Menu.enabled = false;
        startMenu.enabled = true;
    }

    public void NoPressed()
    {
        instructionsMenu.GetComponent<AudioSource>().PlayOneShot(menuClickSound);
        exitMenu.enabled = false;
        playButton.enabled = true;
        exitButton.enabled = true;
    }

    public void backToMainMenuPressed()
    {
        instructionsMenu.GetComponent<AudioSource>().PlayOneShot(menuClickSound);
        startMenu.enabled = true;
        levelMenu.enabled = false;
    }

    public  void PlayPressed()
    {
        instructionsMenu.GetComponent<AudioSource>().PlayOneShot(menuClickSound);
        startMenu.enabled = false;
        levelMenu.enabled = true;

    }

    public void ChinesePressed()
    {
        instructionsMenu.GetComponent<AudioSource>().PlayOneShot(menuClickSound);
        GameManager.LevelSelected = 1;
        SceneManager.LoadScene(1);
    }

    public void FrenchPressed()
    {
        instructionsMenu.GetComponent<AudioSource>().PlayOneShot(menuClickSound);
        GameManager.LevelSelected = 2;
        SceneManager.LoadScene(1);
    }

    public void DutchPressed()
    {
        instructionsMenu.GetComponent<AudioSource>().PlayOneShot(menuClickSound);
        GameManager.LevelSelected = 3;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        instructionsMenu.GetComponent<AudioSource>().PlayOneShot(menuClickSound);
        Application.Quit();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
