using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Enemy
{

    float _distanceToPlayer;

    // Update is called once per frame
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
            }
        }
        
    }


}
