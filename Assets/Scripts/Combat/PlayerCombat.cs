using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Combat
{
    public class PlayerCombat : MonoBehaviour, ActionInterface, ISaveable
    {
        [SerializeField] float  TimeBetweenAttack = 0.85f;
        [SerializeField] Transform RhandTransform = null;
        [SerializeField] Transform LhandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health target;
        float timeSinceLastAttack =  Mathf.Infinity;
        Weapon currentWeapon = null;

        private void Start() 
        {   
            if(currentWeapon == null)
            {
                // If a weapon is Equipped, the weapon will be spawned
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead())    return;
            if (!InRange())
            {
                GetComponent<Mover>().Moveto(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }

        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(LhandTransform, RhandTransform, animator);
        }

        private void AttackBehaviour()
        {
            if(timeSinceLastAttack > TimeBetweenAttack)
            {
                transform.LookAt(target.transform);
                GetComponent<Animator>().SetTrigger("Attack");
                timeSinceLastAttack = 0;
            }
        }

        //  Attack Animation Event
        void Hit()
        {
            if(currentWeapon.hasProjectile())   
                currentWeapon.LaunchProjectile(LhandTransform,RhandTransform, target);
            else
                target.Damage(currentWeapon.GetDamage());
        }

        void Shoot()
        {
            Hit();
        }
        private bool InRange()
        {
            return (Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange());
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget==null)    return false;
            Health testTarget = combatTarget.GetComponent<Health>();
            return (testTarget!=null && !testTarget.IsDead());
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }    

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("StopAttack");
            target = null;
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            // From the captured get the information about the 
            // previously equipped weapon (its name basically)

            string WeaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(WeaponName);

            // Found weapon, Equip it.

            EquipWeapon(weapon);

        }
    }
}