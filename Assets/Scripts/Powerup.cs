using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField] // 0 = Tripleshot, 1 = Speed Boost, 2 = Shields
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;

      // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -7)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if(player != null)
            {
               switch(powerupID)
               {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        Debug.Log("You got a SPEED BOOST!!!");
                        break;
                    case 2:
                        player.ShieldPowerupActive();
                        Debug.Log("You got SHIELDS BABY!!!");
                        break;
               }
                
            }
            Destroy(gameObject);
        }
    }
}
