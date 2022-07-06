using System.Collections;
using UnityEngine;

/*
    Weapon is on the Ground, pick it up 
    and equip it.
*/

namespace RPG.Combat
{
    public class PickupWeapon : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        [SerializeField] float RespawnTime = 4f;
        GameObject player;

        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.tag=="Player")
                {
                    other.GetComponent<PlayerCombat>().EquipWeapon(weapon);
                    StartCoroutine(HideForSeconds(RespawnTime));
                }
        }

        private IEnumerator HideForSeconds(float time)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(time);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }

}