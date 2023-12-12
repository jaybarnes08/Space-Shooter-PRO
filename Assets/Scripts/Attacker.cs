using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Enemy
{

    float _distanceToPlayer;
    float _dodgeSpeedModifier; 
    bool _canDodge = true;

    protected override void Start()
    {
        base.Start();
        _dodgeSpeedModifier = Random.Range(-2, 2);
    }

    protected override void Update()
    {
        base.Update();
  
        
    }
    protected override void CalculateMovement()
    {
        if(_player != null)
        {
            _distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

            if (_distanceToPlayer < 4.4f)
            {
                transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, (_speed * 1.5f) * Time.deltaTime);
            }
            else
            {
                base.CalculateMovement();
                DodgePlayerShot();
            }
        }

        
    }

    void DodgePlayerShot()
    {
        Collider2D[] lasers = Physics2D.OverlapCircleAll(transform.position, 10f, LayerMask.GetMask("Laser"));

        foreach (var laser in lasers)
        {
            float distance = Vector2.Distance(transform.position, laser.transform.position);
            if (distance <= 10f)
            {
                if (_canDodge) {
                    transform.Translate(Vector3.left * (_speed * _dodgeSpeedModifier) * Time.deltaTime);

                    Debug.Log("Dodging");
                }
                
            }
        }
    }

    IEnumerator DodgeCooldownRoutine()
    {
        yield return new WaitForSeconds(3f);
        _canDodge = true;
    }


}
