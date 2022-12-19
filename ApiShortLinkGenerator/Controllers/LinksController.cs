using ApiShortLinkGenerator.Interfaces;
using ApiShortLinkGenerator.Models;
using ApiShortLinkGenerator.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiShortLinkGenerator.Controllers
{
    [Route("/")]
    [ApiController]
    public class LinksController : ControllerBase
    {
        private readonly ILogger<LinksController> _logger;
        private readonly ApplicationContext _context;
        private ITokenGenerator _tokenGenerator;
        private IConfiguration _config;
        private IQRCodeGenerator _qrCodeGenerator;

        public LinksController(ILogger<LinksController> logger, ITokenGenerator generator, ApplicationContext context, IConfiguration configuration, IQRCodeGenerator qrCodeGenerator)
        {
            _logger = logger;
            _tokenGenerator = generator;
            _context = context;
            _config = configuration;
            _qrCodeGenerator = qrCodeGenerator;
        }
        [Route("{token}")]
        [HttpGet]
        public async Task<ActionResult> Get(string token)
        {
            if (!string.IsNullOrEmpty(token)) 
            {
                var Link = _context.Links.Where(el => el.Token == token).FirstOrDefault();
                if (Link != null && !string.IsNullOrEmpty(Link.UserLink)) {
                    Link.DateLastUsed = DateTime.Now;
                    await _context.SaveChangesAsync();
                    if (Uri.TryCreate(Link.UserLink, UriKind.Absolute, out var uri))
                    {
                        return Redirect(UriHelper.Encode(uri));
                    }
                }
            }

            return Problem();
        }
        
        [HttpPost]
        public async Task<ActionResult<LinkView>> Post([FromBody][Url]string userLink)
        {
            if (!string.IsNullOrEmpty(userLink))
            {
                //Получаем адрес приложения
                var url = _config.GetSection("HostUrl").Value ?? "";
                if (string.IsNullOrEmpty(url))
                {
                    var location = new Uri($"{Request.Scheme}://{Request.Host}");
                    url = location.AbsoluteUri;
                }
                string folderForCodes = _config.GetSection("QRCodeFolder").Value ?? "Uploads";

                var link = new Link();
                link.UserLink = userLink;
                //Генерируем новый токен
                link.Token = _tokenGenerator.GetToken();

                //Генерируем ссылку со сгенерированным токеном
                //Генерируем по этой ссылке новый QR и получаем ссылку на него
                var sb = new StringBuilder();
                sb.Append(url);
                sb.Append(link.Token);
                string generatedLink = sb.ToString();
                var qrCodePath = _qrCodeGenerator.CreateAndSaveQRCode(generatedLink, folderForCodes, link.Token);
                sb.Clear();
                sb.Append(url);
                sb.Append(qrCodePath);
                Uri QRUri;
                Uri.TryCreate(sb.ToString(), UriKind.Absolute, out QRUri);
                link.QRPath = QRUri.ToString();
                _context.Links.Add(link);
                await _context.SaveChangesAsync();
                return new LinkView
                {
                    UserLink = link.UserLink,
                    QRPath = link.QRPath,
                    GeneratedLink = generatedLink
                };
            }
            return Problem();
        }
    }
}
