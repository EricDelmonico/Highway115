using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTile : MonoBehaviour
{
    public bool levelEnded;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            levelEnded = true;
        }
    }
}
