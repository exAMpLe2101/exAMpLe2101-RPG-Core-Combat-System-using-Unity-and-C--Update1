using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;
    /*
        Why Needed?

        A bug exists where the player still has the control
        of the playable character when the cutscene is playing.

        How to solve?

        Add observers, these will watch over the completion, pause, or start
        of an action and let you take control of them.

        Observers are similar to interfaces, as they transfer the dependencies.
    */
namespace RPG.Cinematic
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;
        private void Start() {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += ReEnableControl;
            player = GameObject.FindWithTag("Player");
        }

        void DisableControl(PlayableDirector pd)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
            // Player Control Taken away 
        }

        void ReEnableControl(PlayableDirector pd) 
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}