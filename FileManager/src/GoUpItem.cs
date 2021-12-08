using MaterialDesignThemes.Wpf;

namespace FileManager
{
    internal sealed class GoUpItem : GridItem
    {
        public override bool IsEditable => false;

        public GoUpItem()
        {
            this.Icon = new PackIcon() { Kind = PackIconKind.DotsHorizontal };
            this.Text = string.Empty;
        }
    }
}
