namespace CSharp
{
    public class Square : Rectangle
    {
        private int _sidelength;

        public override int Width
        {
            get { return _sidelength; }
            set { _sidelength = value; }
        }

        public override int Height
        {
            get { return _sidelength; }
            set { _sidelength = value; }
        }
    }
}