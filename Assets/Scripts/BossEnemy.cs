using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] GameObject _enemyLaserPrefab;

    [SerializeField] GameObject _laserBeam;

    [SerializeField] GameObject _megaShield;
    [SerializeField] float _currentHealth = 25;
    [SerializeField] float _maxHealth = 25;
    [SerializeField] float _currentShieldHealth = 5;
    [SerializeField] float _maxShieldHealth = 5;
    bool _shieldActive = true;

    int attackIndex;

    UIManager _uiManager;
    Animator _anim;
    AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();

        if (_anim == null)
        {
            Debug.LogError("Animator is null!");
        }

        if(_uiManager == null)
        {
            Debug.LogError("UIManager is null!");
        }

        if(_audio == null)
        {
            Debug.LogError("AudioSource is null!");
        }

        _uiManager.EnableBossUI();

        _currentHealth = _maxHealth;
        _currentShieldHealth = _maxShieldHealth;
        attackIndex = 0;
        StartCoroutine(MoveDownScreenRoutine());
        StartCoroutine(FireLaserRoutine());
        
     
    }

    // Update is called once per frame
    void Update()
    {
        _uiManager.UpdateBossHealth(_currentHealth / _maxHealth, _currentShieldHealth / _maxShieldHealth);
          
    }

    IEnumerator MoveDownScreenRoutine()
    {
        while(transform.position.y  > 3)
        {
            transform.Translate(Vector2.down * _speed * Time.deltaTime);
            yield return null;
        }

    }

    //void CycleAttackRoutine()
    //{
    //    attackIndex = Random.Range(0, 2);

    //    switch (attackIndex)
    //    {
    //        case 0:
    //            StartCoroutine(FireLaserRoutine());
    //            break;
    //        case 1:
    //            StartCoroutine(SideToSideLaserRoutine());
    //            break;
    //    }

    //}

    IEnumerator FireLaserRoutine()
    {
        int shotsFired = 0;

        while(shotsFired < 5)
        {
            GameObject enemylaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemylaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

            shotsFired++;
            yield return new WaitForSeconds(2.3f);
        }

        yield return new WaitForSeconds(2f);
        //CycleAttackRoutine();
        StartCoroutine(SideToSideLaserRoutine());

    }

    IEnumerator SideToSideLaserRoutine()
    {
        float moveRight = 1;
        bool moving = true;
        bool returnToCenter = false;

        _laserBeam.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);

        while (moving)
        {
            transform.Translate(Vector2.right * _speed * moveRight * Time.deltaTime);

            if(transform.position.x >= 8f)
            {
                moveRight = -1f;
            }

            if (transform.position.x <= -8f)
            {
                moveRight = 1f;
                returnToCenter = true;
            }

            if (transform.position.x >= 0 && returnToCenter)
            {
                moveRight = 0;
                yield return new WaitForSeconds(1f);
                _laserBeam.gameObject.SetActive(false);
                moving = false;

            }

            yield return null;
            
        }

        yield return new WaitForSeconds(2f);
        //CycleAttackRoutine();
        StartCoroutine(FireLaserRoutine());
    }

    IEnumerator ShieldCooldownRoutine()
    {
        yield return new WaitForSeconds(8f);
        _currentShieldHealth += _maxShieldHealth;
        _megaShield.gameObject.SetActive(true);
        _shieldActive = true;
        
    }

    public void Damage()
    {
        if (_shieldActive)
        {
            _currentShieldHealth--;

            if(_currentShieldHealth <= 0)
            {
                _megaShield.gameObject.SetActive(false);
                StartCoroutine(ShieldCooldownRoutine());
                _shieldActive = false;
            }
        }
        else
        {
            _currentHealth--;

            if(_currentHealth <= 0)
            {
                DestroyRoutine();
            }
        }

        
    }

    void DestroyRoutine()
    {
        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0;
        GetComponent<Collider2D>().enabled = false;
        _uiManager.GameWonSequence();
        _audio.Play();
        Destroy(this.gameObject, 2.4f);
    }

}
