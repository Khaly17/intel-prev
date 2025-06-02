using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soditech.IntelPrev.Mobile.Models
{
	public class CarouselModel
	{
		private string _image;

		public CarouselModel(string imageString)
		{
			Image = imageString;
		}

		public string Image
		{
			get { return _image; }
			set { _image = value; }
		}

	}
}
