using System.Collections;
using UnityEngine;
using RPG.Saving;

/*
    This Script will act as a higher abstraction level
    for saving/loading/deleting the state of game.
*/

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "SaveFile";
        [SerializeField] float FadeInTime = 0.2f;

        IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutFast();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(FadeInTime);
        }
        
        void Update()
        {
            // Load the existing state
            if(Input.GetKeyDown(KeyCode.L))
                Load();

            // Save the current state
            if(Input.GetKeyDown(KeyCode.S))
                Save();
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }

}