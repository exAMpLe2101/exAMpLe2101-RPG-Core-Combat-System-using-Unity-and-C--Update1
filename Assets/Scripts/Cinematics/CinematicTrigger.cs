using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/*
    When should the cutscene play?
    What should trigger it?
    Moreover, it should not be triggered more than ONCE!
*/

namespace RPG.Cinematic
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool trigger = false;
        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.tag == "Player" && trigger==false)
            {
                GetComponent<PlayableDirector>().Play();
                trigger = true;
            }

        }
    }

}