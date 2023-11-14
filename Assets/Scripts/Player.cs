using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _offset = 1.05f;
    [SerializeField] private float _fireRate = 0.5f;  
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldVisualizer;

    [SerializeField] private GameObject _leftEngine, _rightEngine;
    
    [SerializeField] private float _canFire = -1f;

    [SerializeField] private int _lives = 3;
    [SerializeField] private bool _tripleShotActive = false;
    [SerializeField] private bool _speedBoostActive = false;
    [SerializeField] private bool _shieldsActive = false;
    [SerializeField] private float _speedMultiplier = 2f;

    [SerializeField] private int _score;


    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    [SerializeField] AudioClip _laserAudio;
    AudioSource _audioSource;

    private void Start() {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>(); 

        if(_spawnManager == null) {
            Debug.LogError("SpawnManager is null!");
        }

        if(_uiManager == null) {
            Debug.LogError("UIManager is null!");
        }

        if( _audioSource == null) {
            Debug.LogError("AudioSource on the player is null!");
        }
        else {
            _audioSource.clip = _laserAudio;
        }

    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire) {
            FireLaser();
        }
    }

    void CalculateMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f) {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        } else if (transform.position.x < -11.3f) {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser() {

        _canFire = Time.time + _fireRate;


        if (_tripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _offset, 0), Quaternion.identity);
        }

        //play laser audio clip\
        _audioSource.Play();
        
    }

    public void Damage() {
        if (_shieldsActive) {
            _shieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);

        if (_lives == 2) {
            _leftEngine.SetActive(true);
        } else if (_lives == 1) {
            _rightEngine.SetActive(true);
        }

        if (_lives < 1) {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive() {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine() {
        yield return new WaitForSeconds(5f);
        _tripleShotActive = false;
    }

    public void SpeedBoostActive() {
        _speedBoostActive = true;
        _speed *= _speedMultiplier; 
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine() {
        yield return new WaitForSeconds(5f);
        _speedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive() {
        _shieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int score) {
        _score += score;
        _uiManager.UpdateScore(_score);
    }

    public int GetScore() {
        return _score;
    }

}