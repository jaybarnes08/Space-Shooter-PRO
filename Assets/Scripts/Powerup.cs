using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    //id for powerups
    //0 = triple shot
    //1 = speed 
    //2 = shield
    [SerializeField] int _powerupID;

    [SerializeField] AudioClip _audioClip;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6f) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Player player = collision.gameObject.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_audioClip, transform.position);

            if (player != null) {
                switch (_powerupID) {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        Debug.Log("Shields Active!");
                        break;
                    default:
                        Debug.Log("Default Value!");
                        break;
                }

            
            }


            Destroy(this.gameObject);
        }
    }
}
