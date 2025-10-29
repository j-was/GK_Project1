using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Model.Edges.Constraints
{
    public class ArcConstraint : NoConstraint, IConstraint
    {
        string IConstraint.Symbol => "○";
    }
}
