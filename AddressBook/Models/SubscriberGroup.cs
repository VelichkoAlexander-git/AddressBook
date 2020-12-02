using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Models
{
    public class SubscriberGroup
    {
        public int SubscriberId { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
