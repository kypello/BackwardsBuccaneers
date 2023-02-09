using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct Button {
    public MouseOver mouseOver;
    public TMP_Text text;
    public Color defaultColor;
    public Color highlightColor;

    public bool Check() {
        if (mouseOver.mouseOver) {
            text.color = highlightColor;

            if (Input.GetMouseButtonDown(0)) {
                return true;
            }
        }
        else {
            text.color = defaultColor;
        }
        return false;
    }

    public void SetDefaultState() {
        mouseOver.mouseOver = false;
        text.color = defaultColor;
    }
}

public class PauseMenu : MonoBehaviour, IScrollable
{
    bool paused = false;
    public GameObject pauseUI;
    public PlayerLook playerLook;
    public ShipCam shipCam;

    public Button resumeButton;
    public Button quitButton;

    public TMP_Text sensitivityText;
    public Scrollbar sensitivityScrollbar;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            paused = !paused;

            if (paused) {
                Pause();
            }
            else {
                Unpause();
            }
        }

        if (paused) {
            if (resumeButton.Check()) {
                Unpause();
            }
            
            if (quitButton.Check()) {
                Application.Quit();
            }
        }
    }

    void Pause() {
        paused = true;
        pauseUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerLook.control = false;
        Time.timeScale = 0f;
        sensitivityScrollbar.SetValue(100 - Mathf.FloorToInt(PlayerLook.sensitivity + 0.5f) / 10);
        sensitivityText.text = "Sensitivity: " + (100 - sensitivityScrollbar.GetValue());
    }

    void Unpause() {
        PlayerLook.sensitivity = 1000 - sensitivityScrollbar.GetValue() * 10;
        shipCam.sensitivity = PlayerLook.sensitivity;
        resumeButton.SetDefaultState();
        paused = false;
        pauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerLook.control = true;
        Time.timeScale = 1f;
    }

    public void UpdateScrollValue(int value) {
        sensitivityText.text = "Sensitivity: " + (100 - value);
    }
}
