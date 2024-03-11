using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class BoxColliderExtension
    {
        public static Vector3 RandomPointInBounds(this BoxCollider @this) 
        {
            Bounds bounds = @this.bounds;
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }
        
        public static List<GameObject> GetGameObjectsInBounds(this BoxCollider @this)
        {
            Bounds bounds = @this.bounds;
            Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.extents, @this.transform.rotation);

            return colliders.Select(collider => collider.gameObject).ToList();
        }
    }
}