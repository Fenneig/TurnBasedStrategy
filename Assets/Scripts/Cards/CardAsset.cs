using UnityEngine;

namespace Cards
{
    public enum TargetingOptions
    {
        NoTarget,
        AllCharacters,
        EnemyCharacters,
        YourCharacters,
        Ground
    }
    
    [CreateAssetMenu(fileName = "Create",menuName = "Assets/Cards")]
    public class CardAsset : ScriptableObject
    {
        [Header("General info")] [Tooltip("If this is null, it's a neutral card")]
        public CharacterAsset CharacterAsset;

        [TextArea(2, 3)] public string Description;
        public Sprite SplashImage;

        [Header("Card info")] 
        [Tooltip("If this parameter equals to 0 that means that this card don't have attack action")]
        public int Attack;
        [Tooltip("If this parameter equals to 0 that means that this card don't have defense action")]
        public int Defense;
        [Tooltip("If this parameter equals to 0 that means that this card don't have move action")]
        public int Move;
    }
}