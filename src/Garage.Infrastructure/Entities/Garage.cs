using System.Collections.Generic;

namespace Garage.Infrastructure.Entities
{
    public class Garage : UserBase
    {
        public string Name { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
