using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerSpawnPoint;

    // Start is called before the first frame update
    private void Start()
    {
        _player.transform.position = _playerSpawnPoint.transform.position;
    }
}