using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers
{
    [Route(WebAPIAddrsses.Values)]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly Dictionary<int, string> _Values = Enumerable.Range(1, 10)
            .Select(x => (Id: x, Value: $"Value-{x}"))
            .ToDictionary(v => v.Id, v => v.Value);

        private readonly ILogger<ValuesController> _Logger;

        public ValuesController(ILogger<ValuesController> Logger)
        {
            _Logger = Logger;
        }

        [HttpGet]// GET -> http://localhost:5001/api/values
        public IActionResult Get() => Ok(_Values.Values);

        [HttpGet("{Id}")]// GET -> http://localhost:5001/api/values/5
        public IActionResult GetById(int Id)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            return Ok(_Values[Id]);
        }

        [HttpGet("count")]
        //public IActionResult Count() => Ok(_Values.Count);
        //public ActionResult<int> Count() => _Values.Count; 
        public int Count() => _Values.Count;

        //Можно запрос делать на несколько адресов
        [HttpPost] // POST /api/values
        [HttpPost("add")]// POST /api/values/add
        public IActionResult Add([FromBody]string Value)
        {
            var Id = _Values.Count == 0 ? 1 : _Values.Count+1;
            _Values[Id] = Value;

            return CreatedAtAction(nameof(GetById), new { Id });
        }

        [HttpPut("{Id}")]// GET -> http://localhost:5001/api/values/5
        public IActionResult Replace(int Id, [FromBody]string Value)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            _Values[Id] = Value;

            return Ok();

        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            _Values.Remove(Id);

            return Ok();
        }

    }
}
