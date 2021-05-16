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

        internal async Task<List<AirTicket>> GetTickets() => await _airContext
            .AirTicket
            .OrderBy(t => t.Date.Year)
            .ThenBy(t => t.Date.Month)
            .ThenBy(t => t.Date.Day)
            .ThenBy(t => t.Date.Hour)
            .ThenBy(t => t.Date.Minute)
            .ToListAsync();

        internal async Task GenerateAndCreateTickets(int count)
        {
            if (count < 0 && count > 50)
            {
                throw new Exception("Count should be between 0 and 50.");
            }
            try
            {
                while (count > 0)
                {
                    AirTicket ticket = TicketsRandomGenerator.GetRandomAirTicket();
                    if ((await _airContext.AirTicket.FindAsync(ticket.Code)) is null)
                    {
                        count--;
                        await _airContext.AirTicket.AddAsync(ticket);
                    }
                }
                await _airContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        internal async Task DeleteByPK(AirTicket ticket)
        {
            if (ticket is null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }
            try
            {
                _airContext.AirTicket.Remove(ticket);

                await _airContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        internal async Task CreateTickets(AirTicket ticket)
        {
            try
            {
                if ((await _airContext.AirTicket.FindAsync(ticket.Code)) is null)
                {
                    await _airContext.AirTicket.AddAsync(ticket);

                    await _airContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Code field already used;");
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
