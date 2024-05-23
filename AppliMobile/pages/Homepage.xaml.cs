namespace AppliMobile.pages;

public partial class Homepage : ContentPage
{
	public Homepage()
	{
		InitializeComponent();
	}

    private void GenerateBtn_clicked(object sender, EventArgs e)
    {
		QRCoder.QRCodeGenerator qRCodeGenerator = new QRCoder.QRCodeGenerator();
		QRCoder.QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(InputText.Text, QRCoder.QRCodeGenerator.ECCLevel.L);
		QRCoder.PngByteQRCode qRCode=new QRCoder.PngByteQRCode(qRCodeData);
		byte[] qrCodeBytes = qRCode.GetGraphic(20);
		QrCodeImage.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
    }
}