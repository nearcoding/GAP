using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace GAP.BL
{
    [Serializable]
    public struct Colour
    {
        public byte A;
        public byte R;
        public byte G;
        public byte B;

        public Colour(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public Colour(Color color)
            : this(color.A, color.R, color.G, color.B)
        {
        }
        public Color GetMediaColor()
        {
            return Color.FromArgb(A, R, G, B);
        }

        public static implicit operator Colour(Color color)
        {
            return new Colour(color);
        }

        public static implicit operator Color(Colour colour)
        {
            return Color.FromArgb(colour.A, colour.R, colour.G, colour.B);
        }
    }
}
