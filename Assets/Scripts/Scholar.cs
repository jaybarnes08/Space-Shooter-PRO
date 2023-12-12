using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scholar : Enemy
{
    [SerializeField] bool _canFireAtPowerup = true;
    [SerializeField] GameObject _backLaserPrefab;
    bool _canFireBackLaser = true;

    protected override void Update() {
        base.Update();
        FireAtPowerup();
        FireBackAtPlayer();
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

    private void FireBackAtPlayer()
    {
        //if behind player
        //fire laser up at player
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector2.up, 10f, LayerMask.GetMask("Player"));

        if (hitinfo.collider != null)
        {
            if (_canFireBackLaser)
            {
                Instantiate(_backLaserPrefab, transform.position, Quaternion.identity);
                _canFireBackLaser = false;
                StartCoroutine(FireBackAtPlayerRoutine());
            }
        }

       
    }

    IEnumerator FireAtPowerupRoutine() {
        yield return new WaitForSeconds(5f);
        _canFireAtPowerup = true;
    }

    IEnumerator FireBackAtPlayerRoutine()
    {
        yield return new WaitForSeconds(5f);
        _canFireBackLaser = true;
    }
}
