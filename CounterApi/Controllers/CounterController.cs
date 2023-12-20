using System.Collections.Generic;
using System.Threading.Tasks;
using CounterApi.Data;
using CounterApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IEnumerable<MyCount>> GetTasks()
        {
            return await _context.MyCounts.ToListAsync();
        }
    }
}
