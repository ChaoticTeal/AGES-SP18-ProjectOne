using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
    // Main menu panel
    [SerializeField]
    GameObject mainPanel;
    // Credits panel
    [SerializeField]
    GameObject creditsPanel;
    // Name of next scene
    [SerializeField]
    string nextScene;

    // Start the game
    public void StartButton()
    {
        SceneManager.LoadScene(nextScene);
    }

    // Show the credits panel, hide the main panel
    public void CreditsButton()
    {
        mainPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    // Show the main panel, hide the credits panel
    public void BackButton()
    {
        creditsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    // Quit the game
    public void QuitButton()
    {
        Application.Quit();
    }
}
