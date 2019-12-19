using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlay : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;

    private void Awake()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}
