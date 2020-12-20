using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.DTO
{
    public class AddressDto
    {
        public int Id { get; set; }
        public int? GroupAddressId { get; set; }
        public int AbonentId { get; set; }
        public string Information { get; set; }
    }
}
