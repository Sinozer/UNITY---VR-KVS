using UnityEngine;

namespace Game.Code.Scripts
{
    public class FurnitureScrollerBehavior : MonoBehaviour
    {
        [SerializeField, Range(0, 10)] private float _scrollSpeed = 1;

        private void OnTriggerStay(Collider other)
        {
            other.transform.position += Vector3.forward * (_scrollSpeed * Time.deltaTime);
        }
    }
}