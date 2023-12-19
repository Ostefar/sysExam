using CounterApi.Data;
using CounterApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CounterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        private readonly CounterApiContext _context;

        public CounterController(CounterApiContext context)
        {
            _context = context;
        }

        // GET: api/Counter
        [HttpGet]
        public IEnumerable<MyCount> GetTasks()
        {
            return _context.MyCounts.ToList();
        }
    }
}
