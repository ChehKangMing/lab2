using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public PlayerMovement playerMovement;

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        scoreText.text = "Score: " + score.ToString();
        Debug.Log("Game Over!");
    }

    public void GameOverRestartCallback(){
        playerMovement.RestartGame();
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }
}
