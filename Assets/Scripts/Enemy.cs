using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 4f;
    [SerializeField] GameObject _enemyLaserPrefab;     
    

    Player _player;
    Animator _anim;
    AudioSource _audioSource;
    float _fireRate = 3f;
    float _canFire = -1;

    bool _moveLeft = false;

    private void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null) {
            Debug.LogError("Player is null!");
        }

        if (_anim == null) {
            Debug.LogError("Animator is null!");
        }

        StartCoroutine(ZigZagMovementRoutine());
    }

    void Update()
    {
        //CalculateMovement();
        AlternateMovement();

        if(Time.time > _canFire) {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemylaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemylaser.GetComponentsInChildren<Laser>();
            
            for(int i = 0; i < lasers.Length; i++) {
                lasers[i].AssignEnemyLaser();
                
            }
           
        }
    }

    void CalculateMovement() {
        while (true) {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y < -5f) {
                float randomX = Random.Range(-8f, 8f);
                transform.position = new Vector3(randomX, 7f, 0);
            }
        }
        
    }

    void AlternateMovement() {
        if (_moveLeft) {
            transform.Translate(new Vector3(-1, -1, 0) *  _speed * Time.deltaTime);
        }
        else {
            transform.Translate(new Vector3(1, -1, 0) * _speed * Time.deltaTime);
        }

        if (transform.position.y < -5f) {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7f, 0);
        }


    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player player = other.transform.GetComponent<Player>();

            if (player != null) {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.4f);

        }


        if (other.CompareTag("Laser")) {
            Destroy(other.gameObject);
            if(_player != null) {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.4f);
        }
    }

    IEnumerator ZigZagMovementRoutine() {
        while(true) {
            yield return new WaitForSeconds(3f);
            _moveLeft = !_moveLeft;
            //yield return new WaitForSeconds(3f);
        }
        
    }
}
