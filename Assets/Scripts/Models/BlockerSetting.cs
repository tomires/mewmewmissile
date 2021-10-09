namespace Mew.Models
{
    public class BlockerSetting
    {
        public bool up, down, left, right;

        public BlockerSetting(bool up, bool right, bool down, bool left)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
        }
    }
}
