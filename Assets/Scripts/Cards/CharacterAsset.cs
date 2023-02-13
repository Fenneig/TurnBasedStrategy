using UnityEngine;

namespace Cards
{
    public enum CharClass
    {
        Warrior,
        Priest,
        Rogue,
        Hunter,
        Paladin,
        Mage,
    }
    
    [CreateAssetMenu(fileName = "Create", menuName = "Assets/Character")]
    public class CharacterAsset : ScriptableObject
    {
        public CharClass Class;
        public string CharacterName;
        public int MaxHealth;
        public Sprite AvatarImage;
        public Color32 AvatarBGTint;
        public Color32 CardBGTint;
        public GameObject CharacterPrefab;
    }
}