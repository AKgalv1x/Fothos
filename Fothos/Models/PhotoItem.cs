using SQLite;

namespace Fothos.Models
{
    public class PhotoItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime DateTaken { get; set; }

        public string ImagePath { get; set; } = string.Empty;

        // Propiedad calculada para mostrar la fecha bonita en la UI
        [Ignore] 
        public string FormattedDate => DateTaken.ToString("dd MMM yyyy");
    }
}