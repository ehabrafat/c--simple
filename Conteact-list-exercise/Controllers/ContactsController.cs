using Conteact_list_exercise.DBAccess;
using Conteact_list_exercise.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Conteact_list_exercise.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ContactsController : Controller
    {

        private readonly ContactDbContext _dbContext;
        private readonly ContactRepo _repo;

        public ContactsController(ContactDbContext dbContext, ContactRepo repo) {
            _dbContext = dbContext;
            this._repo = repo;
        }


        [HttpGet]
        public IEnumerable<Person> GetAll() => _repo.GetAll();

        [HttpGet("{id}", Name = nameof(Get))]
        public async Task<IResult> Get(int id)
        {
            try
            {
                var person = await _repo.GetById(id);
                return Results.Ok(person);
            }
            catch(InvalidOperationException)
            {
                return Results.NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> Add([FromBody] PersonDto bodyPerson)
        {
            var person = new Person { Email = bodyPerson.Email, FirstName=bodyPerson.FirstName, LastName = bodyPerson.LastName };
            _dbContext.Contacts.Add(person);
            await _dbContext.SaveChangesAsync();
            return Results.CreatedAtRoute(nameof(Get), new { id = person.Id}, person);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IResult> Delete(int id)
        {
            Person? person = await _dbContext.Contacts.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (person is null) return Results.NotFound();
            return Results.NoContent();
        }

        [HttpGet("findByName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Person>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> FindByName([FromQuery] string name)
        {
             Console.WriteLine(name);
            var res = (from person in _dbContext.Contacts
                       where person.FirstName != null && person.FirstName.Contains(name)
                       || person.LastName != null && person.LastName.Contains(name)
                       select person);

            return Results.Ok(res);

        }
    }
}
