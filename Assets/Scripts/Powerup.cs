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
    Player _player;

    private void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null) {
            Debug.LogError("Player is null!");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, _player.transform.position);

        if (Input.GetKey(KeyCode.C))
        {
            if(distance <= 8f)
                MoveTowardsPlayer();
        }

        if (transform.position.y < -6f) {
            Destroy(this.gameObject);
        }

        
    }

    public void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
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
                        break;
                    case 3:
                        player.RefillAmmo();
                        break;
                    case 4:
                        player.AddLives();
                        break;
                    case 5:
                        player.ClusterBombActive();
                        player.RefillAmmo();
                        break;
                    case 6:
                        player.SlowDownActive();
                        break;
                    case 7:
                        player.ActivatHomingLaser();
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
