using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Defender : Enemy {
    
    [SerializeField] GameObject _enemyShield;
    [SerializeField] bool _shieldActive = true;

    protected override void CalculateMovement() {
        transform.Translate(new Vector3(1f, -.25f, 0f) * _speed * Time.deltaTime);

        if(transform.position.x > 12f) {
            float randomX = Random.Range(-8f, 1f);
            transform.position = new Vector3(randomX, 7f, 0);
        }
    }

    protected override void CalculateEnemyCollision() {
        if(_shieldActive == true) {
            _shieldActive = false;
            _enemyShield.SetActive(false);
            return;
        }

        base.CalculateEnemyCollision();
    }


}
