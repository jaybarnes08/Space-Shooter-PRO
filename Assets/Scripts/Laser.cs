using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float _speed = 8f;
    [SerializeField] bool _isEnemyLaser = false;

    void Update()
    {

        if (!_isEnemyLaser) {
            MoveUp();
        }
        else{
            MoveDown();  
        }  
        
    }

    void MoveUp() {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8) {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(gameObject);
        }
    }

    void MoveDown() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8) {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(gameObject);
        }
    }

    public void AssignEnemyLaser() {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player") && _isEnemyLaser) {
            Player player = collision.GetComponent<Player>();
            if (player != null) {
                player.Damage();

                Destroy(this.gameObject);
            }
        }

        if(collision.CompareTag("Powerup") && _isEnemyLaser) {
            Powerup powerup = collision.GetComponent<Powerup>();
            if(powerup != null) {
                Destroy(powerup.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
