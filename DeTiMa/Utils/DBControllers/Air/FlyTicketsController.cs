using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using DeTiMa.Models.AirDB;

namespace DeTiMa.Utils.DBControllers.Air
{
    class FlyTicketsController
    {
        private readonly DB_AirContext _airContext;

        public FlyTicketsController(DB_AirContext airContext)
        {
            this._airContext = airContext;
        }

        internal async Task<List<AirTicket>> GetTickets() => await _airContext.AirTicket.ToListAsync();
    }
}
