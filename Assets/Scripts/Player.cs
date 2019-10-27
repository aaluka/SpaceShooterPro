using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private float _speedMultiplier = 2f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionSoundClip;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manger is NULL.");
        }
        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }
        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source on the Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    } 

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        // Clamp Method
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5f, 0), 0);

        //If Else Method
        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        
        if(_isTripleShotActive == false)
        {
            //quaternion.idendity is default rotation
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisual.SetActive(false);
            return;
            
        }
        else
        {
            _lives -= 1;
            _uiManager.UpdateLives(_lives);
        }
        

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }

        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }
        
        else if (_lives < 1f)
        {
            _spawnManager.OnPlayerDeath();
            _audioSource.clip = _explosionSoundClip;
            _audioSource.Play();
            Destroy(gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerdown());
    }

    IEnumerator SpeedBoostPowerdown()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldPowerupActive()
    {
        _isShieldActive = true;
        _shieldVisual.SetActive(true);
        
    }  

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
   
}


