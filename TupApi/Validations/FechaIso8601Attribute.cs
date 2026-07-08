using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace TupApi.Validations
{
    /// <summary>
    /// Valida que el string sea una fecha ISO 8601 válida.
    /// Formatos aceptados:
    ///   "2026-07-19"
    ///   "2026-07-19T15:00:00"
    ///   "2026-07-19T15:00:00Z"
    /// </summary>
    public class FechaIso8601Attribute : ValidationAttribute
    {
        private static readonly string[] Formatos = {
            "yyyy-MM-dd",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:ssZ",
            "yyyy-MM-ddTHH:mm:ss.fffZ"
        };

        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            if (value == null) return ValidationResult.Success;

            var texto = value.ToString()!.Trim();

            var esValida = DateTime.TryParseExact(
                texto,
                Formatos,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces,
                out _
            );

            if (!esValida)
                return new ValidationResult(
                    $"El campo '{context.DisplayName}' debe ser una fecha ISO 8601 válida. " +
                    "Ejemplos: '2026-07-19' o '2026-07-19T15:00:00'."
                );

            return ValidationResult.Success;
        }
    }
}