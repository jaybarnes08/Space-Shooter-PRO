using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] GameObject _enemyLaserPrefab;

    [SerializeField] GameObject _laserBeam;

    [SerializeField] GameObject _megaShield;
    [SerializeField] int _health = 25;
    [SerializeField] int _shieldHealth = 5;
    bool _shieldActive = true;

    int attackIndex;

    // Start is called before the first frame update
    void Start()
    {
        _health = 25;
        attackIndex = 0;
        StartCoroutine(MoveDownScreenRoutine());
        StartCoroutine(FireLaserRoutine());
        
     
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    IEnumerator MoveDownScreenRoutine()
    {
        while(transform.position.y  > 3)
        {
            transform.Translate(Vector2.down * _speed * Time.deltaTime);
            yield return null;
        }

    }

    void CycleAttackRoutine()
    {
        attackIndex = Random.Range(0, 2);
        Debug.Log(attackIndex);

        switch (attackIndex)
        {
            case 0:
                StartCoroutine(FireLaserRoutine());
                break;
            case 1:
                StartCoroutine(SideToSideLaserRoutine());
                break;
        }

    }

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
            yield return new WaitForSeconds(3f);
        }

        yield return new WaitForSeconds(3f);
        CycleAttackRoutine();

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
            Debug.Log(moveRight);
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

        yield return new WaitForSeconds(3f);
        CycleAttackRoutine();
    }

    IEnumerator ShieldCooldownRoutine()
    {
        yield return new WaitForSeconds(8f);
        _shieldHealth += 5;
        _megaShield.gameObject.SetActive(true);
        _shieldActive = true;
        
    }

    public void Damage()
    {
        if (_shieldActive)
        {
            _shieldHealth--;

            if(_shieldHealth <= 0)
            {
                _megaShield.gameObject.SetActive(false);
                StartCoroutine(ShieldCooldownRoutine());
                _shieldActive = false;
            }
        }
        else
        {
            _health--;

            if(_health <= 0)
            {
                DestroyRoutine();
            }
        }
        
    }

    void DestroyRoutine()
    {
        Destroy(this.gameObject);
    }

}
