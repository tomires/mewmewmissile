using UnityEngine;
using Mew.Models;

namespace Mew.Objects
{
    public class Mouse : Creature
    {
        [SerializeField] private MeshRenderer mouseRenderer;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material bonusMaterial;
        [SerializeField] private Material modeMaterial;

        public enum MouseType {
            Default, Bonus, Mode
        }

        public MouseType Type => _type;
        private MouseType _type;

        protected override float SpeedMultiplier => Constants.Settings.MouseSpeedMultiplier;

        public override void Initialize(Vector2 coordinates, Direction direction)
        {
            base.Initialize(coordinates, direction);

            _type = GenerateMouseType();
            mouseRenderer.material = _type switch
            {
                MouseType.Bonus => bonusMaterial,
                MouseType.Mode => modeMaterial,
                _ => defaultMaterial
            };
        }

        private MouseType GenerateMouseType()
        {
            var random = Random.Range(0, 100);
            if (random < Mathf.CeilToInt(Constants.Settings.BonusMouseChance * 100f))
                return MouseType.Bonus;
            else if (random < Mathf.CeilToInt((Constants.Settings.BonusMouseChance + Constants.Settings.ModeMouseChance) * 100f))
                return MouseType.Mode;
            else
                return MouseType.Default;
        }
    }
}
