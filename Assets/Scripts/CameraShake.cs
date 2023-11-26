using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float _timeToShake = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateCameraShake() {
        StartCoroutine(CameraShakeRoutine());
    }

    IEnumerator CameraShakeRoutine() {
        
        Vector3 startingPos = transform.position;
        float shakeDuration = _timeToShake;

        while (shakeDuration > 0) {
            Vector3 shakePos = new Vector3(Random.Range(-.1f, .1f), Random.Range(-.1f, .1f), startingPos.z);
            transform.position = shakePos;
            shakeDuration -= Time.deltaTime;
            yield return null;
        }

        transform.position = startingPos;
         

    }
}
