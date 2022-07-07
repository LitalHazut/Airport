using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
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
        private readonly ILiveUpdateRepository<LiveUpdate> _liveUpdateRepository;
        public LiveUpdateService(ILiveUpdateRepository<LiveUpdate> liveUpdateRepository)
        {
            _liveUpdateRepository = liveUpdateRepository;
        }
    }
}
