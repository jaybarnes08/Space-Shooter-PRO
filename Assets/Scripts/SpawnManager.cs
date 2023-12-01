using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject[] _powerup;
    [SerializeField] GameObject[] _rarePowerup;
    
    [SerializeField] int _wave = 1;
    [SerializeField] int _enemiesPerWave;
    int _enemiesSpawned;

    [SerializeField] private bool _stopSpawning = false;

    UIManager _uiManager;

    private void Start() {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_uiManager == null) {
            Debug.LogError("UIManager is NULL");
        }

        _uiManager.UpdateWaveText(_wave);
    }

    private void Update() {
        _enemiesPerWave = _wave * 5;
    }

    IEnumerator SpawnEnemyRoutine() {
        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false) {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesSpawned++;

            if (_enemiesSpawned >= _enemiesPerWave) {
                _stopSpawning = true;
                _enemiesSpawned = 0;
                _wave++;
                _uiManager.UpdateWaveText(_wave);
                _uiManager.NewWaveSequence();
                yield return new WaitForSeconds(6.5f);
                _stopSpawning = false;
            }

            yield return new WaitForSeconds(2.5f);
        }
    }

    IEnumerator SpawnPowerupRoutine() {
        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false) {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 4);
            Instantiate(_powerup[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }

    }

    IEnumerator RarePowerupSpawnRoutine() {
        yield return new WaitForSeconds(Random.Range(8f, 15f));

        while (_stopSpawning == false) {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 0);
            Instantiate(_rarePowerup[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(8f, 15f));
        }
    }

    public void OnPlayerDeath() {
        _stopSpawning = true;
    }

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(RarePowerupSpawnRoutine());
    }

    public void StopSpawning(bool stop) {
        _stopSpawning = stop;
    }

}
