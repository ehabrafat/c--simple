using Microsoft.EntityFrameworkCore;

namespace Conteact_list_exercise.DBAccess
{
    public class ContactRepo
    {

        private readonly ContactDbContext _dbContext;
        public ContactRepo(ContactDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IEnumerable<Person> GetAll() => _dbContext.Contacts;

        public async Task<Person> GetById(int id)
        {
            var person = await _dbContext.Contacts.Where(x => x.Id == id).SingleAsync();
            return person;
        }

    }
}
