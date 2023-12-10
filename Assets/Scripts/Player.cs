using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _thrust = 2f;

    [SerializeField] private float _offset = 1.05f;
    [SerializeField] private float _fireRate = 0.5f;  
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _clusterBombPrefab;
    [SerializeField] private GameObject _homingLaser;

    [SerializeField] private GameObject _leftEngine, _rightEngine;
    
    [SerializeField] private float _canFire = -1f;

    [SerializeField] private int _lives = 3;
    [SerializeField] private bool _tripleShotActive = false;
    [SerializeField] private bool _speedBoostActive = false;
    [SerializeField] private bool _shieldsActive = false;
    [SerializeField] private bool _clusterBombPowerupActive = false;
    [SerializeField] private bool _homingLaserActive = false;
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private int _shieldStrength = 3;

    [SerializeField] private int _score;

    [SerializeField] private int _ammoCount = 15;
    [SerializeField] private int _currentAmmo;

    [SerializeField] private float _maxFuel = 5f;
    [SerializeField] private float _currentFuel = 5f;
    [SerializeField] private bool _thrusterCooldownActive = false;
    [SerializeField] private bool _slowed = false;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    
    private CameraShake _cameraShake;

    [SerializeField] AudioClip _laserAudio;
    AudioSource _audioSource;

    private void Start() {
        _currentAmmo = _ammoCount;

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _cameraShake = Camera.main.GetComponent<CameraShake>();

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

        _uiManager.UpdateAmmoCount(_currentAmmo);
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _currentAmmo > 0) {
            FireLaser();
        }

        if (_shieldsActive) {
            UpdateShieldVisual();
        }

        _uiManager.UpdateAmmoCount(_currentAmmo);
        _uiManager.UpdateLives(_lives);

    }

    private void UpdateShieldVisual() {
        switch (_shieldStrength) {
            case 3:
                _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                break;
            case 2:
                _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .66f);
                break;
            case 1:
                _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .33f);
                break;

        }

    }

    void CalculateMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift)) {
            if (_thrusterCooldownActive == false)
            {
                transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * (_speed * _thrust) * Time.deltaTime);
                _currentFuel -= 1f * Time.deltaTime;

                if(_currentFuel <= 0) {
                    _currentFuel = 0;
                    _thrusterCooldownActive = true;
                    StartCoroutine(ThrusterCooldownRoutine());
                }
            }
            
        }

        if (_slowed) {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * (_speed / 2) * Time.deltaTime);
        } 
        else {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        }

        

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f) {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        } else if (transform.position.x < -11.3f) {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

        _uiManager.UpdateThrusterSlider(_currentFuel / _maxFuel);
    }

    void FireLaser() {

        _canFire = Time.time + _fireRate;

        _currentAmmo--;
        _uiManager.UpdateAmmoCount(_currentAmmo);

        if (_homingLaserActive)
        {
            Instantiate(_homingLaser, transform.position, Quaternion.identity);
        }
        else if (_tripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else if (_clusterBombPowerupActive) {
            Instantiate(_clusterBombPrefab, transform.position, Quaternion.identity);
        }
        else {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _offset, 0), Quaternion.identity);
        }

        //play laser audio clip\
        _audioSource.Play();
        
    }

    public void Damage() {
        if (_shieldsActive) {

            _shieldStrength--;

            if (_shieldStrength < 1) {
                _shieldsActive = false;
                _shieldVisualizer.SetActive(false);
            }

            return;
        }

        _cameraShake.ActivateCameraShake();
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

        _lives = Mathf.Clamp(_lives, 0, 3);
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

    IEnumerator ThrusterCooldownRoutine()
    {
        while (_thrusterCooldownActive) {
            //yield return new WaitForSeconds(1f);
            //_currentFuel += 1f;
            yield return null;
            _currentFuel += 1f * Time.deltaTime;


            if (_currentFuel >= _maxFuel) {
                _currentFuel = _maxFuel;
                _thrusterCooldownActive = false;
            }
        }
    }

    public void ClusterBombActive() {
        _clusterBombPowerupActive = true;
        StartCoroutine(ClusterBombPowerDownRoutine());
    }

    IEnumerator ClusterBombPowerDownRoutine() {
        yield return new WaitForSeconds(5f);
        _clusterBombPowerupActive = false;
    }

    public void ShieldsActive() {
        _shieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }


    public void RefillAmmo() {
        _currentAmmo = _ammoCount;
    }

    public void AddLives() {
        _lives++;
        _lives = Mathf.Clamp(_lives, 0, 3);

        if (_lives == 2) {
            _rightEngine.SetActive(false);
        }
        else if(_lives == 3) {
            _leftEngine.SetActive(false);
        }

    }

    public void SlowDownActive() {
        _slowed  = true;
        StartCoroutine(SlowPowerDownRoutine());
    }

    IEnumerator SlowPowerDownRoutine() {
        yield return new WaitForSeconds(5f);
        _slowed = false;
    }

    public void AddScore(int score) {
        _score += score;
        _uiManager.UpdateScore(_score);
    }

    public int GetScore() {
        return _score;
    }

    public int GetCurrentAmmo() {
        return _currentAmmo;
    }

    public void ActivatHomingLaser()
    {
        _homingLaserActive = true;
        StartCoroutine(HomingLaserPowerDownRoutine());
    }

    IEnumerator HomingLaserPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _homingLaserActive = false;
    }
       

}
