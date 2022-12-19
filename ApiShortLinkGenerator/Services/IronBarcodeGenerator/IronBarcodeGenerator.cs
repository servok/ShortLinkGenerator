using ApiShortLinkGenerator.Interfaces;
using IronBarCode;

namespace ApiShortLinkGenerator.Services.IronBarcodeGenerator
{
    public class IronBarcodeGenerator : IQRCodeGenerator
    {
        private string _webRootPath;
        private string _licenseKey;
        public IronBarcodeGenerator(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _webRootPath = environment.WebRootPath;
            _licenseKey = configuration.GetSection("IronBarcodeLicenseKey").Value ?? "";
        }

        public string CreateAndSaveQRCode(string stringForCoding, string path, string filename)
        {
            string relPath = Path.Combine(path, filename + ".png");
            string fullPath = Path.Combine(_webRootPath, relPath);
            IronBarCode.License.LicenseKey = _licenseKey;
            QRCodeWriter.CreateQrCode(stringForCoding, 500).SaveAsPng(fullPath);
            return relPath;
        }
    }
}
