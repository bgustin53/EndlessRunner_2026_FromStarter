using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreboardText;
    [SerializeField] private TextMeshProUGUI timeRemainingText;
    [SerializeField] private GameObject toggleGroup;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject spawnManager;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ParticleSystem dirtSplatter;
    public static bool gameOver = true;
    private static float score;
    private int timeRemaining = 60;
    private bool timedGame;

    // Update is called once per frame
    void Update()
    {
        DisplayUI();
        EndGame();
    }

    private void DisplayUI()
    {
        scoreboardText.text = "Score: " + Mathf.RoundToInt(score).ToString();
        if (timedGame)
        {
            if (timeRemaining > 0)
            {
                timeRemainingText.text = "Remaining: " + timeRemaining.ToString();
            }
            else
            {
                timeRemainingText.text = "Game Over";
                Time.timeScale = 0;
            }
        }
    }

    private void TimeCountdown()
    {
        timeRemaining--;
    }

    public void StartGame()
    {
        toggleGroup.SetActive(false);
        startButton.SetActive(false);
        if (timedGame)
        {
            timeRemainingText.gameObject.SetActive(true);
            InvokeRepeating("TimeCountdown", 0, 1);
        }
        gameOver = false;
        spawnManager.SetActive(true);
        playerAnimator.SetBool("BeginGame_b", true);
        dirtSplatter.Play();
    }

    private void EndGame()
    {
        if (gameOver || timeRemaining <= 0)
        {
            playerAnimator.SetBool("BeginGame_b", false);
            CancelInvoke();
        }
    }

    public void SetTimed(bool timed)
    {
        timedGame = timed;
    }

    public static void ChangeScore(int change)
    {
        score += change;
        Debug.Log($"Current Score: {score}");
    }
}
