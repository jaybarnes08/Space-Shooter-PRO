using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] GameObject _enemyLaserPrefab;
    int attackIndex;

    // Start is called before the first frame update
    void Start()
    {
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
        switch (attackIndex)
        {
            case 0:
                StartCoroutine(FireLaserRoutine());
                break;
        }

        attackIndex = Random.Range(0, 0);
    }

    //void ResetAttackIndex()
    //{
    //    attackIndex = Random.Range(0, 0);
    //}

    IEnumerator FireLaserRoutine()
    {
        int shotsFired = 0;

        while(shotsFired <= 5)
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
}
