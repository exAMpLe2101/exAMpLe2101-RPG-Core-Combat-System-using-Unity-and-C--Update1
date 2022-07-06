using UnityEngine;

/*
    Why Need a Scheduler script?

    Circular dependencies are very dangerous in runtime.
    Say Movement and Combat dependencies have a mutual dependency 
    on each other. If any of the above has a bug, then both the 
    namespaces will face the issue!

*/

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        ActionInterface currentAction;
        public void StartAction(ActionInterface action)
        {
            // We need to be able to cancel an action at the right time
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}