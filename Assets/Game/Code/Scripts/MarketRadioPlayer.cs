using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class MarketRadioPlayer : MonoBehaviour
{
    // [SerializeField] private AudioSource _audioSource;
    [SerializeField, ReadOnly] private List<AudioSource> _audioSources;
    [SerializeField] private bool _isRandom;
    [SerializeField] private List<AudioClip> _playlist;
    [SerializeField] private AudioClip _announcementSong; 
    [SerializeField] private List<AudioClip> _broadcastMicrophone;
    
    [Header("Announcement")]
    [SerializeField, Range(0, 10)] private float _announcementVolume = 4f;
    [SerializeField, Range(0, 10)] private float _broadcastVolume = 2f;
    [SerializeField, MinValue(0)] private float _minAnnouncementDelay = 20f;
    [SerializeField, MinValue(0)] private float _maxAnnouncementDelay = 60f;
    
    private int _currentTrackIndex;
    private float _baseVolume;
    private bool _isAnnouncementPlaying;
    private bool _isPaused = false;

    private void Awake()
    {
        _audioSources = GetComponentsInChildren<AudioSource>().ToList();
        _baseVolume = _audioSources[0].volume;
    }

    private void Start()
    {
        for (int i = 1; i < _audioSources.Count; i++)
        {
            _audioSources[i].timeSamples = _audioSources[0].timeSamples;
        }
        
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
        }
        
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.Play();
        }
    }

    public void Stop()
    {
        _isPaused = false;

        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.Stop();
        }
    }
    
    public void Pause()
    {
        _isPaused = true;
        
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.Pause();
        }
    }
    
    public void Resume()
    {
        _isPaused = false;
        
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.UnPause();
        }
    }
    
    public void SetVolume(float volume)
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.volume = volume;
        }
    }
    
    
    
    IEnumerator PlayBroadcastMicrophone()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minAnnouncementDelay, _maxAnnouncementDelay));
            yield return new WaitUntil(() => _isPaused == false);
            
            SetVolume(_announcementVolume);
            _isAnnouncementPlaying = true;
            PlayMusic(_announcementSong);
            
            yield return new WaitForSeconds(_announcementSong.length + 1);
            yield return new WaitUntil(() => _isPaused == false);
            
            SetVolume(_broadcastVolume);
            AudioClip randomBroadcast = _broadcastMicrophone[Random.Range(0, _broadcastMicrophone.Count)];
            PlayMusic(randomBroadcast);
            
            yield return new WaitForSeconds(randomBroadcast.length + 1);
            yield return new WaitUntil(() => _isPaused == false);
            
            SetVolume(_baseVolume);
            _isAnnouncementPlaying = false;
        }
    }
}
