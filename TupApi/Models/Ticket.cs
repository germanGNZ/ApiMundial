using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TupApi.Models
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // ─── Datos del partido ───────────────────────────────────────
        [Required(ErrorMessage = "El ID del partido es obligatorio.")]
        public string PartidoId { get; set; } = null!;

        [Range(1, 104)]
        public int NumeroPartido { get; set; }

        public string Equipo1 { get; set; } = "";
        public string Equipo2 { get; set; } = "";
        public string FechaPartido { get; set; } = "";
        public string Estadio { get; set; } = "";
        public string Grupo { get; set; } = "";

        // ─── Datos del comprador ─────────────────────────────────────
        [Required(ErrorMessage = "El nombre del comprador es obligatorio.")]
        [StringLength(100, MinimumLength = 2)]
        public string NombreUsuario { get; set; } = null!;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [StringLength(150)]
        public string EmailComprador { get; set; } = null!;

        [StringLength(20)]
        public string DniComprador { get; set; } = "";

        [StringLength(30)]
        public string TelefonoComprador { get; set; } = "";

        // ─── Datos de la compra ──────────────────────────────────────
        [Required(ErrorMessage = "El sector es obligatorio.")]
        [RegularExpression("^(VIP|General|Platea|Popular)$",
            ErrorMessage = "El sector debe ser VIP, General, Platea o Popular.")]
        public string Sector { get; set; } = null!;

        [Range(1, 100)]
        public int CantidadEntradas { get; set; } = 1;

        public string MetodoPago { get; set; } = "";

        // ─── Desglose de precio ──────────────────────────────────────
        [Range(0, 100000)]
        public decimal Precio { get; set; }          // precio unitario base

        [Range(0, 100000)]
        public decimal Subtotal { get; set; }        // precio × cantidad

        [Range(0, 10000)]
        public decimal CargoServicio { get; set; }   // cargo de servicio (10%)

        [Range(0, 110000)]
        public decimal Total { get; set; }           // subtotal + cargo

        // ─── Metadata ────────────────────────────────────────────────
        public DateTime FechaCompra { get; set; } = DateTime.UtcNow;
    }
}