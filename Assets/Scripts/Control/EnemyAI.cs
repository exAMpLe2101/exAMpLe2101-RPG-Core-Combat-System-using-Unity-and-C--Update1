using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
/*
This script shows the first signs of an enemy AI.
Specifically, when you are in the range of the enemy, 
it will chase you and attack you.
*/
namespace RPG.Control
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float ChaseDistance = 5f;
        [SerializeField] float CooldownTime = 2.5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waitingTime = 1f;
        [Range(0,1)]
        [SerializeField] float PatrolSpeedFraction = 0.4f;  // I'm Patrolling at a relaxed pace

        PlayerCombat fighter;
        GameObject Player;
        Vector3 InitialPosition;
        Mover mover;
        float TimeLastSeenEnemy, TimeSinceArrival;
        int currWayPointIndex = 0;

        Health health;

        private void Start()
        {
            fighter = GetComponent<PlayerCombat>();
            Player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            InitialPosition = transform.position;
            mover = GetComponent<Mover>();
            TimeLastSeenEnemy = Mathf.Infinity;
            TimeSinceArrival = Mathf.Infinity;
        }
        private void Update()
        {
            if (health.IsDead()) return;         //      If you are dead...simply stay put!
            if (InAttackRange() && fighter.CanAttack(Player))
            {
                AttackBehavior();
            }
            else if (TimeLastSeenEnemy < CooldownTime)
            {
                SuspicionBehavior();
                // "I am watching you Wazowski..Always watching." - Monsters Inc.
            }
            else
            {
                PatrolBehaviour();
                // If out of range, just go back to your place.
            }
            UpdateTimer();
        }

        private void AttackBehavior()
        {
            TimeLastSeenEnemy = 0;
            fighter.Attack(Player);
        }

        private void UpdateTimer()
        {
            TimeLastSeenEnemy += Time.deltaTime;
            TimeSinceArrival += Time.deltaTime;
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPos = InitialPosition;
            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    TimeSinceArrival = 0;
                    CycleWaypoint();
                }
                nextPos = GetCurrentWaypoint();
            }
            if (TimeSinceArrival > waitingTime)
            {
                mover.StartMoveAction(nextPos,PatrolSpeedFraction);
            }
        }

        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currWayPointIndex = patrolPath.getNext(currWayPointIndex);

        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWayPoint(currWayPointIndex);
        }
        private bool InAttackRange()
        {
            float v = Vector3.Distance(Player.transform.position, transform.position);
            return v < ChaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, ChaseDistance);
        }
    }
}