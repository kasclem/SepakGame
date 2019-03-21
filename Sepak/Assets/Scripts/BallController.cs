using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class BallController : MonoBehaviour
{
    public static float MAX_TIME = 2.0f;
    public float MAX_FORCE;
    public float MIN_FORCE;
    private Rigidbody2D rb2d;
    private Vector2 offset;
    GameController gc;

    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindObjectOfType<GameController>();
        rb2d = this.GetComponent<Rigidbody2D>();
    }

    private float startTime;
    private bool hadMouseDown = false;
    private void OnMouseDown()
    {
        hadMouseDown = true;
        Vector3 tapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Mathf.Abs(tapPosition.x) < 1.25f)
            return;
        ruleHandler(tapPosition);
        startTime = Time.time;
        rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
        offset = (this.transform.position - tapPosition);
        offset.Normalize();
        ApplyTorque(offset);

        //ApplyTorque must come before the following:
        rb2d.velocity = Vector2.zero;
    }

    private int ctr = 0;
    private void ruleHandler(Vector3 tapPosition)
    {
        Player lastTap = Player.Player2;
        if (tapPosition.x < 0)
        {
            lastTap = Player.Player1;
        }

        if (lastTap == gc.currentPlayer)
        {
            ctr++;
        }
        else
        {
            ctr = 1;
        }
        gc.currentPlayer = lastTap;
        
        //gc.charge() has dependency on gc.currentPlayer:
        gc.charge();
        if (ctr > 3)
            gc.violateRule(gc.currentPlayer);
    }

    internal void resetCount()
    {
        this.ctr = 0;
        hadMouseDown = false;
    }

    private void OnMouseUp()
    {
        if (!hadMouseDown)
            return;
        rb2d.constraints = RigidbodyConstraints2D.None;
        float elapsedTime = Time.time - startTime;
        float multiplier = 0.0f;
        if (elapsedTime > MAX_TIME)
        {
            multiplier = MAX_FORCE + MIN_FORCE;
        }
        else
        {
            multiplier = MIN_FORCE + elapsedTime / MAX_TIME * MAX_FORCE;
        }
        rb2d.AddForce(offset * multiplier, ForceMode2D.Impulse);
        gc.stopCharging();
    }

    private void ApplyTorque(Vector2 offset)
    {
        Vector3 cross = Vector3.Cross(rb2d.velocity, offset);
        rb2d.AddTorque(-cross.z * 0.5f, ForceMode2D.Impulse);
    }

    //private void OnMouseDown()
    //{
    //    rb2d.bodyType = RigidbodyType2D.Dynamic;

    //    Vector2 offset = (this.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));
    //    offset.Normalize();
    //    print(offset);
    //    ApplyTorque(offset);
    //    rb2d.velocity = Vector2.zero;
    //    offset.Set(offset.x, offset.y + 0.10f);
    //    rb2d.AddForce(offset * force, ForceMode2D.Impulse);
    //}

    //private void ApplyTorque(Vector2 offset)
    //{
    //    Vector3 cross = Vector3.Cross(rb2d.velocity, offset);
    //    rb2d.AddTorque(-cross.z * 0.5f, ForceMode2D.Impulse);
    //}
}
