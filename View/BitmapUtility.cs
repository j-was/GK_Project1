using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.View
{
    public static class BitmapUtility
    {
        public static void AddPixel(Bitmap b,  int x, int y,Color c)
        {
            if(x>=0 && y>=0&&x<b.Width&&y<b.Height)
            {
                b.SetPixel(x,y,c);
            }
        }
    }
}
