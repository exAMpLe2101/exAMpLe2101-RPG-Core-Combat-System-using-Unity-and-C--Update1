using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/*
    This script is responsible for the so called "teleportation"
    mechanic where the player transports to a different region upon 
    entering certain area.
    Here, upon interacting with a box collider, a new scene is loaded
*/

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,B,C,D,E
        }

        [SerializeField] int SceneToLoad = -1;
        [SerializeField] Transform SpawnPoint;
        [SerializeField] DestinationIdentifier Destination;
        [SerializeField] float FadeInTime = 1f;
        [SerializeField] float FadeOutTime = 3f;
        [SerializeField] float FadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other) {
            if(other.tag=="Player")
                StartCoroutine(Transition());
        }

        public IEnumerator Transition()
        {
            if(SceneToLoad <0)
            {
                Debug.LogError("Scene To Load Not Set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(FadeOutTime);

            //  Save Current Level
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(SceneToLoad);

            //  Load Current Level
            wrapper.Load();


            Portal otherPortal = getOtherPortal();
            UpdatePlayer(otherPortal);

            wrapper.Save();

            yield return new WaitForSeconds(FadeWaitTime);
            yield return fader.FadeIn(FadeInTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.SpawnPoint.position;
            player.transform.rotation = otherPortal.SpawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal getOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this)  continue;
                if (portal.Destination != Destination) continue;
                return portal;
            }
            return null;
        }
    }

}