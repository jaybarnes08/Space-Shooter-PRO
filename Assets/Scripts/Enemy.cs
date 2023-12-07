using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float _speed = 4f;
    [SerializeField] protected GameObject _enemyLaserPrefab;     
    

    protected Player _player;
    protected Animator _anim;
    protected AudioSource _audioSource;
    protected float _fireRate = 3f;
    protected float _canFire = -1;

    protected virtual void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null) {
            Debug.LogError("Player is null!");
        }

        if (_anim == null) {
            Debug.LogError("Animator is null!");
        }
        
    }

    protected virtual void Update()
    {
        CalculateMovement();
        if (Time.time > _canFire) {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            FireLaser();
        }

    }

    protected void FireLaser() {        
        GameObject enemylaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemylaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++) {
            lasers[i].AssignEnemyLaser();

        }

    }

    protected virtual void CalculateMovement() {
        
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7f, 0);
        }
        
    }

    

    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player player = other.transform.GetComponent<Player>();

            if (player != null) {
                player.Damage();
            }

           CalculateEnemyCollision();

        }


        if (other.CompareTag("Laser")) {
            Destroy(other.gameObject);
            if(_player != null) {
                _player.AddScore(10);
            }

            CalculateEnemyCollision();
        }
    }

    protected virtual void CalculateEnemyCollision() {
        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0;
        GetComponent<Collider2D>().enabled = false;
        _audioSource.Play();
        Destroy(this.gameObject, 2.4f);
    }


}
