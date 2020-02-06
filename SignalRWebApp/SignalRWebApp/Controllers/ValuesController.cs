using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace SignalRWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class ValuesController : ControllerBase
    {
        private IHubContext<SignalHub> _hub;
        public ValuesController(IHubContext<SignalHub> hub)
        {
            _hub = hub;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _hub.Clients.All.SendAsync("clientMethodName", "get all called");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{connectionId}")]
        public ActionResult<string> Get(string connectionId)
        {
            _hub.Clients.Client(connectionId).SendAsync("clientMethodName", "get called");
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
