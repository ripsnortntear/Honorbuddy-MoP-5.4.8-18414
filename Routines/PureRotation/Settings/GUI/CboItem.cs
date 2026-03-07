namespace PureRotation.Settings.GUI
{
    public class CboItem
    {
        public readonly int E;
        private readonly string _s;

        public CboItem(int pe, string ps)
        {
            E = pe;
            _s = ps;
        }

        public override string ToString()
        {
            return _s;
        }
    }
}