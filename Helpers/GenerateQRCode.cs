using iText.Barcodes;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout.Element;

namespace IText8PdfPOC.Helpers
{
    public static class GenerateQRCode
    {
        public static Image GetQRCodeImage(string text,Color qrColor, PdfDocument document)
        {
            //Create a barcode object
            var qrCode = new BarcodeQRCode();
            qrCode.SetCode(text);
            var qrCodeObject = qrCode.CreateFormXObject(qrColor, document);
            return new Image(qrCodeObject);
        }
    }
}
