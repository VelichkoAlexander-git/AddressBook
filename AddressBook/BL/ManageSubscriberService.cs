using AddressBook.DTO;
using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.BL
{
    public class ManageSubscriberService
    {
        private readonly AddressBookContext _subscriberContext;

        public ManageSubscriberService(AddressBookContext subscriberContext)
        {
            _subscriberContext = subscriberContext;
        }
        public async Task<Result<bool>> AddSubscriberAsync(int UserId, SubscriberDto subscriber)
        {

            // validate data

            // perform additional actions

            await _subscriberContext.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
