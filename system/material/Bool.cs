namespace Butterfly
{
    public class Bool
    {
        private bool _is;

        public Bool() { _is = false; }
        public Bool(bool value) { _is = value; }

        public bool Value { get { return _is; } }

        public void True() => _is = true;
        public void False() => _is = false;
    }
}
