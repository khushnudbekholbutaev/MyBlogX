using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPostify.Domain.Commons;

[Keyless,ComplexType]
public class MultyLanguageField
{
    public string Uz { get; set; }
    public string Ru { get; set; }
    public string Eng { get; set; }
    public string Tr { get; set; } // turk
}
