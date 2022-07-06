using UnityEngine;

/*
    This script deals with all the basic characterics
    of some preset characters, similar to Dark Souls and Bloodborne
    from FromSoftware games.

    Here, we assign some basic stats for the players like level, strength,
    immunity, HP, etc.
*/

namespace RPG.Stats
{

    public class BasicStats : MonoBehaviour
    {
        [Range(1, 10)] 
        [SerializeField] int Level = 7;
        [SerializeField] CharacterClass Class;
        [SerializeField] Progression progression = null;

        public float GetHP()
        {
            return progression.GetHealth(Class, Level);
        }
    }

}