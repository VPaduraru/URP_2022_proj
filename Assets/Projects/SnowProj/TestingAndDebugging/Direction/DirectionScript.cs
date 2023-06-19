namespace Testing
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DirectionScript : MonoBehaviour
    {
        [SerializeField]
        private Transform _point1;
        [SerializeField]
        private Transform _point2;
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_point1.position, .1f);
            Gizmos.DrawSphere(_point2.position, .1f);

            Vector3 direction = _point1.position - _point2.position;
            Gizmos.DrawRay(_point1.position, -direction.normalized);
        }
    }
}

