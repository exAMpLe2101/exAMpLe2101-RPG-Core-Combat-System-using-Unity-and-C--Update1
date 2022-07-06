/*
    Why need an Interface?

    This will eliminate the possible circular dependencies
    that could be possible between scheduler and movement, 
    as well as scheduler and combat namespaces respectively.
*/

namespace RPG.Core
{
    public interface ActionInterface {
        void Cancel();
    }
}