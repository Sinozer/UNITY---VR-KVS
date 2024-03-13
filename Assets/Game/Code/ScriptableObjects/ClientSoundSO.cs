using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClientSoundSO", menuName = "ScriptableObjects/ClientSoundSO", order = 1)]
public class ClientSoundSO : ScriptableObject
{
    [SerializeField] private List<AudioClip> _audioClipList;
    
    public List<AudioClip> AudioClipList => _audioClipList;
}
