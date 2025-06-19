namespace Soditech.IntelPrev.Mobile.Models;

public class CarouselModel
{
	public CarouselModel(string imageString)
	{
		Image = imageString;
	}

	public string Image { get; set; }
}