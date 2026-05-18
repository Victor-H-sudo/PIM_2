using Microsoft.AspNetCore.Mvc;
using EcoPoint.Data;

namespace EcoPoint.Controllers
{
    public class ControllerBaseEcoPoint : Controller
    {
        protected readonly ApplicationDbContext _context;

        public ControllerBaseEcoPoint(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}