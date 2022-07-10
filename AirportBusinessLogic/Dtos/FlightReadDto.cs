using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBusinessLogic.Dtos
{
    public class FlightReadDto
    {
        public int FlightId { get; set; }
        public bool IsAscending { get; set; }
        public bool IsDone { get; set; }
        public DateTime InsertionTime { get; set; }

        
    }
}
