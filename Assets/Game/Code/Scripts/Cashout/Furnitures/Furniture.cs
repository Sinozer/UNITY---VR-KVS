using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Game.Code.Scripts
{
    public struct ShelfItem
    {
        public int ID;
        public ItemSo ItemSo;
        public int MaxItemNumber;
        public int ItemNumber;
    }
    
    public class Furniture : MonoBehaviour
    {
        public ItemSo ProductSo => _productSo;
        public bool IsGrabbed => _isGrabbed;
        
        private ItemSo _productSo;
        private bool _isGrabbed;

        private void Start()
        {
            XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
            grabInteractable.firstFocusEntered.AddListener(Grab);
            grabInteractable.focusExited.AddListener(Release);
        }

        public void Initialize(ItemSo product)
        {
            _productSo = product;
        }
        
        public void Grab(FocusEnterEventArgs args)
        {
            _isGrabbed = true;
        }
        
        public void Release(FocusExitEventArgs args)
        {
            _isGrabbed = false;
        }
    }
}