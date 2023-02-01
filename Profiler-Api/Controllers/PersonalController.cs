using Microsoft.AspNetCore.Mvc;
using Profiler_Api.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Profiler_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonalController : ControllerBase
{
    private SchedulerContext db;
    public PersonalController(SchedulerContext db)
    {
        this.db = db;
    }

    // GET: api/<PersonalController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return this.db.Users.Select(u => u.Username).ToList();
    }

    // GET api/<PersonalController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<PersonalController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<PersonalController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<PersonalController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
