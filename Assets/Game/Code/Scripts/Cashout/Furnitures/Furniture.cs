using UnityEngine;

namespace Game.Code.Scripts
{
    public class Furniture : MonoBehaviour
    {
        public ItemSo ProductSo => _productSo;
        public bool CanBeGrabbed => _isGrabbed;
        
        
        private ItemSo _productSo;
        private bool _isGrabbed;
        
        
        public void Initialize(ItemSo product)
        {
            _productSo = product;
        }
    }
}