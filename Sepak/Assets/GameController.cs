using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum Player
    {
        Player1,
        Player2
    }
    private int lScoreInt;
    private int rScoreInt;
    public Text lScore;
    public Text rScore;
    public GameObject ball;
    public Transform servePosition;
    private Rigidbody2D rb2d;
    public Player currentPlayer;
    public BarController p1Bar;
    public BarController p2Bar;

    internal void leftScore()
    {
        lScoreInt++;
        updateScore();
    }

    internal void rightScore()
    {
        rScoreInt++;
        updateScore();
    }

    internal void charge()
    {
        if (currentPlayer == Player.Player1)
        {
            p1Bar.isCharging = true;
        }
        else
            p2Bar.isCharging = true;
    }

    private void updateScore()
    {
        lScore.text = lScoreInt.ToString();
        rScore.text = rScoreInt.ToString();
    }

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = ball.GetComponent<Rigidbody2D>();
        ResetGame();
    }

    public void ResetGame()
    {
        lScore.text = "0";
        rScore.text = "0";
        lScoreInt = 0;
        rScoreInt = 0;
        ResetBall();
        stopCharging();
    }

    public void ResetBall() {
        ball.GetComponent<BallController>().resetCount();
        rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0;
        print(String.Format("{0}, {1}", lScoreInt, rScoreInt));
        //setCurrent Player:
        if((lScoreInt + rScoreInt)%2 == 0)
        {
            currentPlayer = Player.Player1;
            ball.transform.position = servePosition.position;
            print("play1");
        }
        else
        {
            currentPlayer = Player.Player2;
            ball.transform.position = new Vector2(-servePosition.position.x, servePosition.position.y);
            print("play2");
        }
        stopCharging();
    }

    internal void violateRule(Player currentPlayer)
    {
        if (currentPlayer == Player.Player1)
        {
            this.rightScore();
        }
        else
        {
            this.leftScore();
        }
        this.ResetBall();
    }

    internal void stopCharging()
    {
        BarController[] bcs = GameObject.FindObjectsOfType<BarController>();
        foreach(BarController bc in bcs)
        {
            bc.reset();
        }
    }
}
