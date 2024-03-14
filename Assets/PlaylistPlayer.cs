using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaylistPlayer : MonoBehaviour
{
    // [SerializeField] private AudioSource _audioSource;
    [SerializeField, ReadOnly] private List<AudioSource> _audioSources;
    [SerializeField] private bool _isRandom;
    [SerializeField] private List<AudioClip> _playlist;
    [SerializeField] private AudioClip _announcementSong; 
    [SerializeField] private List<AudioClip> _broadcastMicrophone;
    
    private int _currentTrackIndex;
    private bool _isAnnouncementPlaying;

    private void Awake()
    {
        _audioSources = GetComponentsInChildren<AudioSource>().ToList();
    }

    private void Start()
    {
        _currentTrackIndex = _isRandom ? Random.Range(0, _playlist.Count) : 0;
        PlayMusic(_currentTrackIndex);
        StartCoroutine(PlayBroadcastMicrophone());
    }
    
    private void Update()
    {
        if (_audioSources[0].isPlaying || _isAnnouncementPlaying) return;
        
        if (_isRandom)
        {
            PlayRandomTrack();
        }
        else
        {
            PlayNextTrack();
        }
    }
    
    [Button]
    public void PlayNextTrack()
    {
        _currentTrackIndex++;
        if (_currentTrackIndex >= _playlist.Count)
        {
            _currentTrackIndex = 0;
        }
        
        PlayMusic(_currentTrackIndex);
    }
    
    [Button]
    public void PlayPreviousTrack()
    {
        _currentTrackIndex--;
        if (_currentTrackIndex < 0)
        {
            _currentTrackIndex = _playlist.Count - 1;
        }
        
        PlayMusic(_currentTrackIndex);
    }
    
    [Button]
    public void PlayRandomTrack()
    {
        int newIndex;

        if (_playlist.Count <= 1)
        {
            newIndex = 0;
        }
        else
        {
            do
            {
                newIndex = Random.Range(0, _playlist.Count);
            } 
            while (newIndex == _currentTrackIndex);
        }
        
        _currentTrackIndex = newIndex;
        PlayMusic(_currentTrackIndex);
    }
    
    public void PlayMusic(int index)
    {
        if (index < 0 || index >= _playlist.Count)
        {
            Debug.LogError("Invalid clip index!");
            return;
        }
        
        _currentTrackIndex = index;
        PlayMusic(_playlist[_currentTrackIndex]);
    }
    
    private void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("Invalid clip!");
            return;
        }

        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        // _audioSource.clip = clip;
        // _audioSource.Play();
    }

    public void Stop()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.Stop();
        }
        // _audioSource.Stop();
    }
    
    public void Pause()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.Pause();
        }
        // _audioSource.Pause();
    }
    
    public void Resume()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.UnPause();
        }
        // _audioSource.UnPause();
    }
    
    public void SetVolume(float volume)
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.volume = volume;
        }
        // _audioSource.volume = volume;
    }
    
    IEnumerator PlayBroadcastMicrophone()
    {
        while (true)
        {
            //yield return new WaitForSeconds(Random.Range(60, 125));
            yield return new WaitForSeconds(5);
            Pause();
            _isAnnouncementPlaying = true;
            PlayMusic(_announcementSong);
            
            yield return new WaitForSeconds(_announcementSong.length + 1);
            
            AudioClip randomBroadcast = _broadcastMicrophone[Random.Range(0, _broadcastMicrophone.Count)];
            PlayMusic(randomBroadcast);
            
            yield return new WaitForSeconds(randomBroadcast.length + 1);
            _isAnnouncementPlaying = false;
        }
    }
}
