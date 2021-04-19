using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePrototype : MonoBehaviour
{
    public Vector2 direction;

    private void Start()
    {
        StartCoroutine(DestroyInTime());
    }

    private void Update()
    {
        transform.position += (Vector3)direction * 4 * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
            Destroy(gameObject);
        //else if (collision.gameObject.GetComponent<Enemy>() != null)
        //{
        //    collision.gameObject.GetComponent<Enemy>().Damage(1);
        //    Destroy(gameObject);
        //}
    }

    IEnumerator DestroyInTime()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
