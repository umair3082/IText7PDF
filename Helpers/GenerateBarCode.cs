using iText.Barcodes;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout.Element;

namespace IText8PdfPOC.Helpers
{
    public static class GenerateBarCode
    {
        public static Image GetBarcodeImage(string text,Color BarCodcolor,Color BarcodTextColor,PdfDocument document)
        {
            //Create a barcode object
            Barcode128 barcode = new Barcode128(document);
            //Set the code to be encoded
            barcode.SetCode(text);
            //Set the code type
            barcode.SetCodeType(Barcode128.CODE128);
            //Set the height of the barcode
            barcode.SetBarHeight(15);
            //PDF from XObject
            PdfFormXObject barcodeObject= barcode.CreateFormXObject(BarCodcolor, BarcodTextColor,document); 
            //Create an image object
            var barcodeImage = new Image(barcodeObject);
            return barcodeImage;

        }
    }
}
