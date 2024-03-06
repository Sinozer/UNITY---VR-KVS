using System;
using System.Linq;
using Game.Code.Scripts.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Code.Scripts
{
    public class FurnitureScrollerBehavior : MonoBehaviour
    {
        private BoxCollider _scrollArea;
        
        private void Start()
        {
            _scrollArea = GetComponent<BoxCollider>();
        }

        private void Update()
        {
            foreach (var furniture in _scrollArea.GetGameObjectsInBounds().Where(furniture => !furniture.CompareTag("Static")))
            {
                furniture.transform.position += Vector3.forward * Time.deltaTime;
            }
        }
        
        
    }
}