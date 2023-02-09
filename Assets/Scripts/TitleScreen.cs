using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    public static bool playedAlready = false;
    public static int previousScore = 0;
    public static int bestScore = 0;

    public Button playButton;
    public Button quitButton;
    
    public TMP_Text scoreText;
    public Animation whiteOut;
    bool loadingScene = false;

    void Awake() {
        if (playedAlready) {
            scoreText.text = "Score: " + previousScore + "\n<size=48>Best: " + bestScore;
            playButton.text.text = "Play Again";
        }
        else {
            scoreText.text = "";
        }
    }

    void Update() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playButton.Check() && !loadingScene) {
            StartCoroutine(StartGame());
        }

        if (quitButton.Check()) {
            Application.Quit();
        }
    }

    IEnumerator StartGame() {
        loadingScene = true;
        whiteOut.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
}
