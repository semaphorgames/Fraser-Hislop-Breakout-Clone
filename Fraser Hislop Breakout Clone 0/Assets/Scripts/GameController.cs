using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Singleton Instance
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    // Caching
    private GUIController guiController;
    private BricksController bricksController;

    private List<Ball> ballPool = new List<Ball>();

    // Score and Lives
    private int score = 0;
    public int Score { get { return score; } }
    private int scorePerBrick = 100;
    private int lives;
    [SerializeField] [Range(1, 10)]
    private int livesMax = 3;

    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;

    }

    private void Start()
    {
        guiController = GUIController.Instance;
        bricksController = BricksController.Instance;

        guiController.SetScoreText(score);
        lives = livesMax;
        guiController.SetLivesText(lives);
    }

    public void AddBall(Ball ball)
    {
        ballPool.Add(ball);
    }

    public void RemoveBall(Ball ball)
    {
        ballPool.Remove(ball);
    }

    public void IncreaseScore()
    {
        score += scorePerBrick;
        guiController.SetScoreText(score);
    }

    public void ResetScore()
    {
        score = 0;
        guiController.SetScoreText(score);
    }

    // After running out of lives, return balls and reset speed, replace bricks, and reset lives and score
    public void DecrementLives()
    {
        lives--;
        
        if (lives <= 0)
        {
            foreach (Ball ball in ballPool) ball.FirstRound();

            bricksController.ReplaceBricks();

            lives = livesMax;
            ResetScore();
        }

        guiController.SetLivesText(lives);
    }

    // After clearing Bricks, return balls and increase speed, and replace bricks
    public void NextRound()
    {
        foreach (Ball ball in ballPool) ball.NextRound();

        bricksController.ReplaceBricks();
    }
}
