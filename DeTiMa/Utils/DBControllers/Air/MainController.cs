using DeTiMa.Models.AirDB;

namespace DeTiMa.Utils.DBControllers.Air
{
    class MainController
    {
        private readonly static DB_AirContext _airContext = new();
        internal static FlyTicketsController FlyTickets { get; } = new(_airContext);
    }
}
