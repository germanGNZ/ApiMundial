using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TupApi.Models
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        // Nunca se devuelve al cliente
        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = null!;

        public string Rol { get; set; } = "usuario";

        public DateTime FechaRegistro { get; set; }
    }
}