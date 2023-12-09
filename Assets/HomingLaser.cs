using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingLaser : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    float _distanceToClosestEnemy = Mathf.Infinity;
    GameObject[] _enemies;
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        

        if(_enemies.Length == 0)
        {
            transform.Translate(Vector2.up * _speed * Time.deltaTime);
        }
        else
        {
            foreach(var enemy in _enemies) { 
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

                if(distance < _distanceToClosestEnemy)
                {
                    _distanceToClosestEnemy = distance;
                    target = enemy.transform;
                }
            }

        transform.position = Vector2.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);

        }

        

       




    }
}
