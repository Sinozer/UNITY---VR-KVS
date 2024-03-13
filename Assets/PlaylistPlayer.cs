using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaylistPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private bool _isRandom;
    [SerializeField] private List<AudioClip> _playlist;
    
    private int _currentTrackIndex;
    

    private void Start()
    {
        _currentTrackIndex = _isRandom ? Random.Range(0, _playlist.Count) : 0;
        PlayMusic(_currentTrackIndex);
    }
    
    private void Update()
    {
        if (_audioSource.isPlaying) return;

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
        
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }
    
    public void Pause()
    {
        _audioSource.Pause();
    }
    
    public void Resume()
    {
        _audioSource.UnPause();
    }
    
    public void SetVolume(float volume)
    {
        _audioSource.volume = volume;
    }
}
