using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetector : MonoBehaviour
{
    public GameController gc;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.name == "LeftDetector")
        {
            gc.rightScore();
        }
        else
        {
            gc.leftScore();
        }

        gc.ResetBall();
    }
}
