using UnityEngine;

/*
    This Script is responsible for the main fading of the screen
    upon entering and exiting the scenes. The reason for implementing
    this script to add a little flexibility in spawning the persistent 
    objects (in this case, the 'fader' script).
*/

namespace RPG.Core
{

    public class PersistentObjectOwner : MonoBehaviour
    {
        [SerializeField]    GameObject PersistentObjectPrefab;

        static bool hasSpawned = false;

        private void Awake() {
            if(hasSpawned)  return;

            SpawnPersistentObjects();

            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(PersistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }

}