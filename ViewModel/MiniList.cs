using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace catamorphism
{
    class MiniList : INotifyPropertyChanged
    {
        public BitmapImage _icon;
        public string _shortwebpagename;
        public string _login;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

		public void Refresh()
		{
			OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
		}

		public MiniList(string icon, string shortPage, string login)
		{
			if (!File.Exists(icon))
			{
				_icon = getEmptyBitmap();
			}
			else
			{
				_icon = new BitmapImage(new Uri(icon));
			}

			_shortwebpagename = shortPage;
			_login = login;
		}

        public BitmapImage Icon
        {
            get { return _icon; }
        }
        public string ShortName
        {
            get { return _shortwebpagename; }
        }
        public string Login
        {
            get { return _login; }
        }
		private BitmapImage getEmptyBitmap()
		{
			int width = 128;
			int height = width;
			int stride = width / 8;
			byte[] pixels = new byte[height * stride];

			// Try creating a new image with a custom palette.
			List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
			colors.Add(System.Windows.Media.Colors.Red);
			colors.Add(System.Windows.Media.Colors.Blue);
			colors.Add(System.Windows.Media.Colors.Green);
			BitmapPalette myPalette = new BitmapPalette(colors);

			// Creates a new empty image with the pre-defined palette
			BitmapSource image = BitmapSource.Create(
													 width, height,
													 96, 96,
													 System.Windows.Media.PixelFormats.Indexed1,
													 myPalette,
													 pixels,
													 stride);


			BitmapSource bitmapSource = image;

			JpegBitmapEncoder encoder = new JpegBitmapEncoder();
			MemoryStream memoryStream = new MemoryStream();
			BitmapImage bImg = new BitmapImage();

			encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
			encoder.Save(memoryStream);

			memoryStream.Position = 0;
			bImg.BeginInit();
			bImg.StreamSource = memoryStream;
			bImg.EndInit();

			memoryStream.Close();

			return bImg;
		}
    }
}
