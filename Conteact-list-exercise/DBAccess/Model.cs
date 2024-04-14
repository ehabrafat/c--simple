using Microsoft.AspNetCore.Mvc;

namespace Conteact_list_exercise.DBAccess
{
    public class Person 
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;

    }

}
