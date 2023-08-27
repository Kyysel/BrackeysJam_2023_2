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
    public Sound playerMoveSound;
    private AudioSource headAudioSource;
    //private float initVolume;
    private bool volumeIncreasing;

    private void Awake()
    {
        //initVolume = headAudioSource.volume;
        //headAudioSource.volume = 0;

        headAudioSource = GetComponent<AudioSource>();

    }

    private void Start()
    {
        AudioManager.instance.PlaySoundOnTarget(playerMoveSound, transform);
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
            StartCoroutine(AudioManager.instance.Fade(headAudioSource, 0.5f, playerMoveSound.soundSettings.volume, false));
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
            StartCoroutine(AudioManager.instance.Fade(headAudioSource, 1f, 0, false));
        }
        
        volumeIncreasing = false;
    }

}
