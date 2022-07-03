using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBusinessLogic.Models
{
    public class Plane
    {
        public int PlaneID { get; set; }
        public string PlaneName { get; set; }

        public Plane(int planeID, string planeName)
        {
            PlaneID = planeID;
            PlaneName = planeName;

        }
    }
}
