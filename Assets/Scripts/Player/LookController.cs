using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LookController : MonoBehaviour
{
    [Header("Movement Properties")]
    public float moveSpeed;
    public float rotationSpeed;
    private Vector2 direction;

    [Header("Sounds")]
    public BasicSound playerMoveSound;
    private AudioSource headAudioSource;
    //private float initVolume;
    private bool volumeIncreasing;
    private PlayerController _pc;

    private void Awake()
    {
        //initVolume = headAudioSource.volume;
        //headAudioSource.volume = 0;

        headAudioSource = GetComponent<AudioSource>();

    }

    private void Start()
    {
        AudioManager.instance.PlaySoundBaseOnTarget(playerMoveSound, transform, false);
        _pc = GetComponentInParent<PlayerController>();
    }

    void FixedUpdate()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        
        direction = cursorPos - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, cursorPos, moveSpeed * Time.deltaTime);
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        if (Vector2.Distance(transform.position, cursorPos) > 0.05f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            StartMovementSound();
        }
        else
        {
            EndMovementSound();
        }
    }

    private void StartMovementSound()
    {

        if (volumeIncreasing) 
        {
            return;
        }

        if (headAudioSource.volume < playerMoveSound.soundSettings.volume)
        {
            StopAllCoroutines();
            StartCoroutine(AudioManager.instance.FadeMusic(headAudioSource, 0.5f, playerMoveSound.soundSettings.volume, false));
        }
        
        volumeIncreasing = true;
    }
    private void EndMovementSound()
    {

        if (!volumeIncreasing)
        {
            return;
        }

        if (headAudioSource.volume > 0)
        {
            StopAllCoroutines();
            StartCoroutine(AudioManager.instance.FadeMusic(headAudioSource, 1f, 0, false));
        }
        
        volumeIncreasing = false;
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        // if it collides with a resourceDeposit, add the corresponding resource to resourceDict in ResourceManager
        if (other.CompareTag("ResourceDeposit"))
        {
            ResourceDeposit rd = other.GetComponent<ResourceDeposit>();
            rd.CollectResource(_pc.collectAmount);
            CameraShake.instance.ShakeOnce(0.3f, 0.2f, 0.1f);

        }
        if (other.CompareTag("Worm"))
        {
            WormController wc = other.GetComponentInParent<WormController>();
            ResourceManager.Instance.ChangeResource("wormonium", _pc.collectAmount);
            wc.TakeDamage(_pc.damage);
            CameraShake.instance.ShakeOnce(0.5f, 0.2f, 0.2f);
        }
    }

    public void PlayCollectionAnimation(ParticleSystem particleSystem)
    {
        ParticleSystem aniObject = Instantiate(particleSystem, transform.position, particleSystem.transform.rotation);
        Destroy(aniObject.gameObject, particleSystem.main.duration);
    }

}
