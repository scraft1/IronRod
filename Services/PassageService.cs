using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace IronRod.Services
{
    public class PassageService {
        private ILogger<PassageService> _logger; 
        public PassageService(ILogger<PassageService> logger)
        {
            _logger = logger;
        }
    }
}
