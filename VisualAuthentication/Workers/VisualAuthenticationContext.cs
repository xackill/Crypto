using System.Data.Entity;
using Core.Workers;
using VisualAuthentication.DataBaseModels;

namespace VisualAuthentication.Workers
{
    public class VisualAuthenticationContext : Context
    {
        public DbSet<Session> Sessions { get; set; }
    }
}