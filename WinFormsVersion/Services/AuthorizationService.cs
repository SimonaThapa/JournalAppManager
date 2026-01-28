using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimsAppJournal.Services
{
    public class AuthorizationService
    {
        private const string Pin = "1234";

        public bool ValidatePin(string input)
        {
            return input == Pin;
        }
    }
}
