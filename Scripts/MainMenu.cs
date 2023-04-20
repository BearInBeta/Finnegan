using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject credits;
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newGame()
    {
        audio.PlayOneShot(clip);

        PlayerPrefs.DeleteAll();
        loadGameScene();

    }
    public void Continue()
    {
        audio.PlayOneShot(clip);

        loadGameScene();


    }

    private void loadGameScene()
    {

        SceneManager.LoadScene("GameScene");

    }

    public void showCredits()
    {
        audio.PlayOneShot(clip);

        credits.SetActive(!credits.activeInHierarchy);
    }

    public void exitGame()
    {
        audio.PlayOneShot(clip);

        Application.Quit();
    }

    public void mainMenu()
    {
        audio.PlayOneShot(clip);

        SceneManager.LoadScene("MainMenu");
    }
}
