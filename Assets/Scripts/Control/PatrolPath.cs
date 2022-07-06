using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float Radius = 0.5f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = getNext(i);
                Gizmos.DrawSphere(GetWayPoint(i), Radius);
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
            }

        }

        public int getNext(int i)
        {
            if (i + 1 == transform.childCount) return 0;
            return (i + 1);
        }

        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }

}