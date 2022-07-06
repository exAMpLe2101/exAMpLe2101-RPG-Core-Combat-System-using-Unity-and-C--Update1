using System.Collections;
using UnityEngine;

/*
    While transitioning from one scene to another,
    the screen will fade-in and fade-out, later giving
    the control to the player.
*/

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        private void Start() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutFast()
        {
            canvasGroup.alpha = 1;
            
        }
        public IEnumerator FadeOut(float time)
        {
            while(canvasGroup.alpha < 1)
            {
                // In simple words, alpha is the smallest step of
                // the total time in which the fading process occurs

                
                canvasGroup.alpha += Time.deltaTime/time;
                //  Moving alpha towards one
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)   //  update alpha
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}