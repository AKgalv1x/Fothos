using SQLite;

namespace Fothos.Models
{
    // El atributo Table define el nombre exacto de la tabla en SQLite
    [Table("tbl_user")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique] // No puede haber dos usuarios con el mismo nombre
        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
    }
}