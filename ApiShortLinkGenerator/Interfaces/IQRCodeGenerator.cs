namespace ApiShortLinkGenerator.Interfaces
{
    public interface IQRCodeGenerator
    {
        string CreateAndSaveQRCode(string stringForCoding, string path, string filename);
    }
}
