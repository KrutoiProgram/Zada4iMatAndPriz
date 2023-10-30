using System.ComponentModel.DataAnnotations;

namespace ApiJwtDemo.Models
{
    #nullable disable
    public class User
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(256)]
        public string Password { get; set; }

        [StringLength(256)]
        public string Salt { get; set; }
    }
}
