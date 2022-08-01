using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornTrap : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameController controller = GameObject.Find("GameController").GetComponent<GameController>();
            controller.LoseGame();
        }
    }
}
