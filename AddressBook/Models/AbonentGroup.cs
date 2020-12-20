using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Models
{
    public class AbonentGroup
    {
        public int AbonentId { get; set; }
        public virtual Abonent Abonent { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
