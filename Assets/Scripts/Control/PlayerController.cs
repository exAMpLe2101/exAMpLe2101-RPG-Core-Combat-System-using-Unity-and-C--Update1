using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using RPG.Resources;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        Health health;
        private void Start()
        {
            health = GetComponent<Health>();
        }
        private void Update()
        {
            if (health.IsDead()) return; // If you're dead, you can't do jack T_T

            if (CombatInteraction()) return;
            if (MovementInteraction()) return;
            //print("Nothing to do!");
        }

        private bool CombatInteraction()
        {
            // Raycast all objects so as to know that player
            // wants to attack the enemy even if the fixed-camera
            // obscures the view of the enemy. 
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {

                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponent<PlayerCombat>().CanAttack(target.gameObject)) continue;         // If we can't attack, continue

                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<PlayerCombat>().Attack(target.gameObject);
                }
                return true;        //  If our ray hits an object that could be interacted
                // with combat, it returns true. 

                // This is helpful to differentiate which objects in the world are to be
                // attacked and otherwise.
            }
            return false;
        }
        private bool MovementInteraction()
        {
            //  Implementing Click-to-Move mechanic
            RaycastHit hit;
            bool has_hit = Physics.Raycast((Ray)GetMouseRay(), out hit);
            if (has_hit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}