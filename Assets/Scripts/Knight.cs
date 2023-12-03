using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Enemy
{

    bool _moveLeft = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(ZigZagMovementRoutine());
    }

    protected override void CalculateMovement()
    {
        if (_moveLeft)
        {
            transform.Translate(new Vector3(-1, -1, 0) * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(1, -1, 0) * _speed * Time.deltaTime);
        }

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7f, 0);
        }
    }

    IEnumerator ZigZagMovementRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            _moveLeft = !_moveLeft;
            
        }

    }

}
