using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starpower : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().StarPowerActive(3);
            Destroy(gameObject);
        }
    }
}
