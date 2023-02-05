using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score;
    public TMP_Text scoreText;
    public Animation scoreAnim;
    public Animation gainAnim;
    public Animation loseAnim;

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
}
