using System;
using UnityEngine;

namespace Game.Code.Scripts
{
    public class FurnitureBehavior : MonoBehaviour
    {
        private void Start()
        {
            
        }

        public bool CanBeGrabbed => _isGrabbed;
        
        private bool _isGrabbed;
    }
}