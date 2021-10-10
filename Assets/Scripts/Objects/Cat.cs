namespace Mew.Objects
{
    public class Cat : Creature
    {
        protected override float SpeedMultiplier => Constants.Settings.CatSpeedMultiplier;
    }
}
