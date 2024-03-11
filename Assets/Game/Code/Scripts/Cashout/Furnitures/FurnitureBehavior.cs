using System;
using UnityEngine;

public class FurnitureBehavior : MonoBehaviour
{
    private void Start()
    {
            
    }

    public bool CanBeGrabbed => _isGrabbed;
        
    public ItemSo So { get; set; }

    private bool _isGrabbed;
}