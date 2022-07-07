using AirportBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBusinessLogic.Services
{
    public class LiveUpdateService : ILiveUpdateService
    {
        private readonly ILiveUpdateService _liveUpdateService;
        public LiveUpdateService(ILiveUpdateService liveUpdateService)
        {
            _liveUpdateService = liveUpdateService;
        }
    }
}
