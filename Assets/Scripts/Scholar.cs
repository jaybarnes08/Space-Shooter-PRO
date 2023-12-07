using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scholar : Enemy
{
   [SerializeField] bool _canFireAtPowerup = true;

    protected override void Update() {
        base.Update();
        FireAtPowerup();
    }

    private void FireAtPowerup() {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position + new Vector3(0, -1f, 0), Vector2.down, 10f);

        if (hitInfo.collider != null) {
            if (hitInfo.collider.CompareTag("Powerup")) {

                if (_canFireAtPowerup) {
                    FireLaser();
                    _canFireAtPowerup = false;
                    StartCoroutine(FireAtPowerupRoutine());
                }

            }

        }
    }

    IEnumerator FireAtPowerupRoutine() {
        yield return new WaitForSeconds(5f);
        _canFireAtPowerup = true;
    }
}
