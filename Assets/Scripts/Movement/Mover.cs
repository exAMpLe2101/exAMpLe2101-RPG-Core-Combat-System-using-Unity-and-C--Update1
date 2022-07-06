using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, ActionInterface, ISaveable
    {
        [SerializeField] Transform target;  // What is my target?
        [SerializeField] float maxSpeed = 6;
        NavMeshAgent navMeshAgent;      // Responsible for Movement
        Health health;                  // HP

        private void Start()
        {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            Moveto(destination, speedFraction);
        }

        public void Moveto(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }


        private void UpdateAnimator()   // Convert the global velocity 
        {
            Vector3 vel = navMeshAgent.velocity;
            Vector3 local_vel = transform.InverseTransformDirection(vel);
            //  Why are we converting the global velocity to local velocity?
            /*
                When we are creating our velocity vector,we are collecting 
                global coordinates..implying global velocity.
                Whereas, our animator just wants to know if we are running forward or not.
                This way, we can tell our animator that we are running in forward direction. 
            */
            float speed = local_vel.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles); 
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }

}