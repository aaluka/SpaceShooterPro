using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioClip _explosion;
    [SerializeField]
    private AudioSource _audioSource;

    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Anim is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The Enemy audio source is NULL!");
        }
        _audioSource.clip = _explosion;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -6.5f)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 6f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
     if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
            _speed = 0;
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            Destroy(this.gameObject, 2.3f);

        }

    if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(10);
            }
            _speed = 0;
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(GetComponent<Collider2D>());
            _audioSource.Play();
            Destroy(this.gameObject, 2.3f);
        }

    }
}
