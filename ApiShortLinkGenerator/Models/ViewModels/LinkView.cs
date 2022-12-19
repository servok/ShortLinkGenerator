using System.ComponentModel.DataAnnotations;

namespace ApiShortLinkGenerator.Models.ViewModels
{
    public class LinkView
    {
        public string? UserLink { get; set; }
        public string? GeneratedLink { get; set; }
        public string? QRPath { get; set; }
    }
}
