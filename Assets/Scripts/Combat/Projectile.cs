using RPG.Resources;
using UnityEngine;

/*
    Arrow Physics, look at the target and move there
*/

namespace RPG.Combat
{

    public class Projectile : MonoBehaviour
    {
        Health target = null;
        [SerializeField] float speed = 1f;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject HitEffect = null;
        [SerializeField] float MaxLifetime = 8;
        [SerializeField] GameObject[] DestroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;
        float damage = 0;

        private void Start() {
            transform.LookAt(GetAimLocation()); //  This line is responsible for the homing behavior
        }
        
        // Every time a frame is rendered, the following code will run.
        private void Update() {
            if(target==null)    return;

            if(!target.IsDead() && isHoming)    transform.LookAt(GetAimLocation());

            transform.Translate(Vector3.forward * speed* Time.deltaTime); 
        }

        public void SetTarget(Health target,float damage)
        {
            this.target = target;
            this.damage = damage;

            Destroy(gameObject,MaxLifetime);    
            // Instead of hogging the Heirarchy, the effects 
            // would be destroyed after some certain time period.
        }

        //  Stop shooting at my feet, the body is up here!
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetcaspsule = target.GetComponent<CapsuleCollider>();
            if(targetcaspsule==null)    return target.transform.position;
            return (target.transform.position + Vector3.up * targetcaspsule.height/2);
            //  Vector3.up * targetcaspsule.height/2 = The offset for the Guts!

        }

        private void OnTriggerEnter(Collider other) {
            if(other.GetComponent<Health>() != target)    return;
            if(target.IsDead()) return;
            target.Damage(damage);
            
            speed = 0;      // after hitting, get lost you arrow.

            if(HitEffect != null)
                Instantiate(HitEffect,GetAimLocation(),transform.rotation);

            foreach(GameObject toDestroy in DestroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }

    }

}