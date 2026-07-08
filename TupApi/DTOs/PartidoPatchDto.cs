using System.ComponentModel.DataAnnotations;

namespace TupApi.DTOs
{
    /// <summary>
    /// DTO para PATCH /api/partido/{id}
    /// Solo campos que pueden cambiar tras definirse los equipos
    /// de una llave eliminatoria.
    /// </summary>
    public class PartidoPatchDto
    {
        [StringLength(60, MinimumLength = 2,
            ErrorMessage = "El nombre del equipo debe tener entre 2 y 60 caracteres.")]
        public string? Equipo1 { get; set; }

        [StringLength(60, MinimumLength = 2,
            ErrorMessage = "El nombre del equipo debe tener entre 2 y 60 caracteres.")]
        public string? Equipo2 { get; set; }

        public string? Flags1 { get; set; }
        public string? Flags2 { get; set; }

        public bool? Definido { get; set; }
    }
}