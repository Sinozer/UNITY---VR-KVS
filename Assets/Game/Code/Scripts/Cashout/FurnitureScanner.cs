using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureScanner : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private void OnTriggerEnter(Collider other)
    {
        _audioSource.Play();
    }
}
