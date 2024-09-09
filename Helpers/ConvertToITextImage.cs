using iText.IO.Image;
using iText.Layout.Element;

namespace IText8PdfPOC.Helpers
{
    public static class ConvertToITextImageClass
    {
        public static Image ConvertToITextImage(System.Drawing.Image drawingImage)
        {
            // Convert System.Drawing.Image to a byte array
            using (var ms = new MemoryStream())
            {
                drawingImage.Save(ms, drawingImage.RawFormat);
                byte[] imageBytes = ms.ToArray();

                // Create iText ImageData object from the byte array
                var imageData = ImageDataFactory.Create(imageBytes);

                // Create iText Image element
                return new iText.Layout.Element.Image(imageData);
            }
        }
        public static iText.Layout.Element.Image ConvertBytesToITextImage(byte[] imageBytes)
        {
            // Create iText ImageData object from the byte array
            var imageData = ImageDataFactory.Create(imageBytes);

            // Create iText Image element
            return new iText.Layout.Element.Image(imageData);
        }
    }
}
