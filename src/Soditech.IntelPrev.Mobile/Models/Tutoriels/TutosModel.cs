using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soditech.IntelPrev.Mobile.Models.Tutoriels
{
	public class TutosModel
	{
		public class Video
		{
			public string Title { get; set; }
			public string Thumbnail { get; set; } // URL or file path for the thumbnail
			public string VideoUrl { get; set; }
		}
	}
}
