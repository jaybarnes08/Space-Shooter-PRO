using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartGameText;
    [SerializeField] private TMP_Text _ammoCountText;
    [SerializeField] private Slider _thrusterSlider;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();

        if( _gameManager == null) {
            Debug.LogError("Game Manager is null!");
        }

        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);

    }

    public void UpdateScore(int playerScore) {
        _scoreText.text = "Score :" + playerScore;
    }

    public void UpdateLives(int currentLives) {
        _livesImage.sprite = _livesSprites[currentLives];

        if(currentLives == 0) {
            GameOverSequence();
           
        }
    }

    public void UpdateAmmoCount(int ammo) {
        _ammoCountText.text = "Current Ammo: " + ammo;
    }

    public void UpdateThrusterSlider(float fuelPercentage)
    {
        _thrusterSlider.value = fuelPercentage;
    }

    void GameOverSequence() {
        _restartGameText.gameObject.SetActive(true);
        _gameManager.GameOver();
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine() {
        while (true) {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
            
        }
        
    }
}
