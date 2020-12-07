using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.DTO
{
    public class PhoneDto
    {
        public int Id { get; set; }
        public int? GroupPhoneId { get; set; }
        public int SubscriberId { get; set; }
        public string Number { get; set; }
    }
}
