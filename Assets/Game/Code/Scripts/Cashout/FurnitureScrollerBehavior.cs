using UnityEngine;

namespace Game.Code.Scripts
{
    public class FurnitureScrollerBehavior : MonoBehaviour
    {
        [SerializeField, Range(0, 10)] private float _scrollSpeed = 1;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Furniture furniture) is false) return;
            
            if (furniture.IsGrabbed) return;
            
            other.transform.position += Vector3.forward * (_scrollSpeed * Time.deltaTime);
        }
    }
}