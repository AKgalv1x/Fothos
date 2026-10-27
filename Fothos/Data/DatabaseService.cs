using SQLite;
using Fothos.Models;
using System.Security.Cryptography;
using System.Text;

namespace Fothos.Data
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _db;

        public DatabaseService()
        {
            // Ruta donde se guardará el archivo de la base de datos en el celular
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "galeria.db3");

            _db = new SQLiteAsyncConnection(dbPath);
        }

        // Inicializar tablas y crear usuario por defecto
        public async Task InitAsync()
        {
            await _db.CreateTableAsync<User>();
            await _db.CreateTableAsync<PhotoItem>();

            // Si no hay usuarios, crear el "admin" por defecto
            var users = await _db.Table<User>().ToListAsync();
            if (users.Count == 0)
            {
                await _db.InsertAsync(new User
                {
                    Username = "admin",
                    FullName = "Administrador",
                    PasswordHash = HashPassword("123456") // Contraseña por defecto
                });
            }
        }

        // --- MÉTODOS DE LOGIN ---
        public async Task<User?> LoginAsync(string username, string password)
        {
            string hash = HashPassword(password);
            return await _db.Table<User>()
                .Where(u => u.Username == username && u.PasswordHash == hash)
                .FirstOrDefaultAsync();
        }

        // --- MÉTODOS DE FOTOS (CRUD) ---
        public Task<List<PhotoItem>> GetPhotosAsync() =>
            _db.Table<PhotoItem>().OrderByDescending(p => p.DateTaken).ToListAsync();

        public Task<int> SavePhotoAsync(PhotoItem photo) =>
            photo.Id != 0 ? _db.UpdateAsync(photo) : _db.InsertAsync(photo);

        public Task<int> DeletePhotoAsync(PhotoItem photo) =>
            _db.DeleteAsync(photo);

        // --- FUNCIÓN DE HASH ---
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}