namespace SpookSuite.Menu.Core
{
    internal class MenuTab : MenuFragment
    {
        public string name;
        public bool isDebug;

        public MenuTab(string name, bool isDebug = false)
        {
            this.name = name;
            this.isDebug = isDebug;
        }

        public virtual void Draw() { }
    }
}
