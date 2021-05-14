using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using DeTiMa.Models.AirDB;
using DeTiMa.Utils;

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

        internal async Task GenerateAndCreateTickets(int count)
        {
            if (count < 0 && count > 50)
            {
                throw new Exception("Count should be between 0 and 50.");
            }
            try
            {
                while (count-- > 0)
                {
                    await _airContext.AirTicket.AddAsync(TicketsRandomGenerator.GetRandomAirTicket());
                }
                await _airContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
