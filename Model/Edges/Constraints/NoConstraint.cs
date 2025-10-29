using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace PolygonEditor.Model.Edges.Constraints
{
    public class NoConstraint : IConstraint
    {
        public string Symbol => "";


        public void CorrectEdgeCCW()
        {
            return;
        }

        public void CorrectEdgeCW()
        {
            return;
        }

        public void CorrectEdgeMed()
        {
            return;
        }

        public bool IsSatisfied()
        {
            return true;
        }

        public void SetToEdge(IEdge edge)
        {
            return;
        }
    }
}
