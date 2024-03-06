using System.Collections;
using System.Collections.Generic;
using Game.Code.Scripts.Extensions;
using UnityEngine;

namespace Game.Code.Scripts
{
    public class FurnitureSpawner : MonoBehaviour
    {
        [SerializeField, Range(0, 10)] private float spawnRate;
        [SerializeField] private List<GameObject> furniturePrefabs;
        
        private BoxCollider _spawnArea;
        private Coroutine _spawnCoroutine;
        
        private void Start()
        {
            _spawnArea = GetComponent<BoxCollider>();
            _spawnCoroutine = StartCoroutine(SpawnFurniture());
        }
        
        private IEnumerator SpawnFurniture()
        {
            while (true)
            {
                Vector3 spawnPoint = _spawnArea.RandomPointInBounds();
                GameObject randomFurniture = furniturePrefabs[Random.Range(0, furniturePrefabs.Count)];
                Instantiate(randomFurniture, spawnPoint, Quaternion.identity);
                yield return new WaitForSeconds(spawnRate);
            }
        }
    }
}