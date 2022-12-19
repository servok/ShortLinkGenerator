using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ApiShortLinkGenerator.Models
{
    [Index("Token", IsUnique = true)]
    public sealed class Link
    {
        public Link()
        {
            DateCreated = DateLastUsed = DateTime.Now;

        }

        public int Id { get; set; }
        [Url]
        public string? UserLink { get; set; }
        public string? Token { get; set; }
        public string? QRPath { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUsed { get; set; }
    }
}
