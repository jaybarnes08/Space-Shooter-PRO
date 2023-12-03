using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float _speed = 2f;
    [SerializeField] GameObject _laserCluster;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BombExplosionRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    IEnumerator BombExplosionRoutine() {
        yield return new WaitForSeconds(1f);
        Instantiate(_laserCluster, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
