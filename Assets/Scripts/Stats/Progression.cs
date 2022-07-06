using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "Stats/Progression", order = 0)]
public class Progression : ScriptableObject 
{
    [SerializeField] ProgressionCharacterClass[] characterClasses = null;

    public float GetHealth(CharacterClass Class, int BasicLevel)
    {
        foreach(ProgressionCharacterClass progressionclass in characterClasses)
        {
            if(progressionclass.characterClass == Class)
            {
                return progressionclass.HP[BasicLevel-1];
            }
        }

        return 30;
    }

    [System.Serializable]

    class ProgressionCharacterClass
    {
        public CharacterClass characterClass; 
        public float[] HP;
    }
}