using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTile : MonoBehaviour
{
    public Boss boss;
    public bool levelEnded;

    // Update is called once per frame
    void Update()
    {
        // doesn't seem to trigger, not sure is it because the boss is gone or due to gameObject not being active from start
        if (boss.health <= 0)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            levelEnded = true;
        }
    }
}
