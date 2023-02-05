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

    public MouseOver playButton;
    public TMP_Text playButtonText;
    public Color defaultColor;
    public Color highlightColor;
    public TMP_Text scoreText;
    public Animation whiteOut;
    bool loadingScene = false;

    void Awake() {
        if (playedAlready) {
            scoreText.text = "Score: " + previousScore + "\n<size=48>Best: " + bestScore;
            playButtonText.text = "Play Again";
        }
        else {
            scoreText.text = "";
        }
    }

    void Update() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playButton.mouseOver || loadingScene) {
            playButtonText.color = highlightColor;
            if (Input.GetMouseButtonDown(0) && !loadingScene) {
                StartCoroutine(StartGame());
            }
        }
        else {
            playButtonText.color = defaultColor;
        }
    }

    IEnumerator StartGame() {
        loadingScene = true;
        whiteOut.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
}
