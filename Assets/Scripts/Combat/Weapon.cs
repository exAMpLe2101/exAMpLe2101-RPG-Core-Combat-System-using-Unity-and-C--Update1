using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Create New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] float WeaponRange = 1f;

        [SerializeField] float WeaponDamage = 8f;

        [SerializeField] GameObject WeaponPrefab = null;

        // The following SerializeField is responsible for overriding the
        // default animator for changing attack animations (unarmed to weapon).
        [SerializeField] AnimatorOverrideController AnimationOverride = null;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        //  By Default, Setting my weapons' name as "Weapon"
        const string WeaponName = "weapon"; 

        public bool hasProjectile()
        {
            return projectile!=null;
        }

        public void LaunchProjectile(Transform LHand, Transform RHand, Health target) 
        {
            Projectile proj_instance = Instantiate(projectile, GetTransform(LHand, RHand).position, Quaternion.identity);
            proj_instance.SetTarget(target, WeaponDamage);
        }

        public void Spawn(Transform Lefthand, Transform Righthand, Animator animator)
        {
            // Check whether player already has a weapon in his hands, if he does, destroy it!
            DestroyOldWeapon(Lefthand,Righthand);

            if(WeaponPrefab != null)
            {
                Transform handtransform = GetTransform(Lefthand, Righthand);
                GameObject weapon = Instantiate(WeaponPrefab, handtransform);
                weapon.name = WeaponName;
            }

            var OverrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if(AnimationOverride != null)   
            // Each weapon has its own animations, so it is required to override the default animations
            // from the animation controller.
                animator.runtimeAnimatorController = AnimationOverride;
            
            else if(OverrideController != null)
            {
                //  Return your parent animation override controller
                animator.runtimeAnimatorController = OverrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform lefthand, Transform righthand)
        {
            Transform oldweapon = righthand.Find(WeaponName);
            if(oldweapon == null)
                oldweapon = lefthand.Find(WeaponName);
            if(oldweapon == null)   return;
            // Well, you don't have any weapons, find one and pick one...

            oldweapon.name = "Destroyed";
            Destroy(oldweapon.gameObject);
            // You can't wield 2 different weapons at once (say, a bow in one hand and a sword in another)
            // Destroy one before picking another.
        }

        private Transform GetTransform(Transform Lefthand, Transform Righthand)
        {
            Transform handtransform;
            if (isRightHanded) handtransform = Righthand;
            else handtransform = Lefthand;
            return handtransform;
        }

        public float GetRange()
        {
            return WeaponRange;
        }

        public float GetDamage()
        {
            return WeaponDamage;
        }
    }
}