using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Score : MonoBehaviour
{
    public int score;
    public TMP_Text scoreText;
    public Animation scoreAnim;
    public Animation gainAnim;
    public Animation loseAnim;

    public TMP_Text timerText;

    public Animation whiteOut;

    float time = 240f;

    bool unloading = false;

    void Update() {
        if (time <= 0f) {
            if (!unloading) {
                unloading = true;

                if (!TitleScreen.playedAlready) {
                    TitleScreen.playedAlready = true;
                    TitleScreen.bestScore = score;
                }
                else if (TitleScreen.bestScore < score) {
                    TitleScreen.bestScore = score;
                }
                TitleScreen.previousScore = score;

                StartCoroutine(ToTitleScreen());
            }
        }
        else {
            time -= Time.deltaTime;
            if ((int)time % 60 < 10) {
                timerText.text = (int)time / 60 + ":0" + (int)time % 60;
            }
            else {
                timerText.text = (int)time / 60 + ":" + (int)time % 60;
            }
        }
    }

    public void ChangeScore(int amount) {
        score += amount;
        scoreText.text = "" + score;
        scoreAnim.Play();
        if (amount == 1) {
            gainAnim.Play();
        }
        else if (amount == -1) {
            loseAnim.Play();
        }
    }

    IEnumerator ToTitleScreen() {
        whiteOut.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
    }
}
