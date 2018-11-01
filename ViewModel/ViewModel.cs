using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using MahApps.Metro.Controls;
using System.Windows.Data;
using System.Timers;

namespace catamorphism
{
    class ViewModel : INotifyPropertyChanged
    {
		private ViewWebsiteData pd;
		private List<MiniList> mini;
		private Model.Vault vault;
		private Timer otpTimer;
		private System.Threading.Thread blkThread;

		public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
		public void Load(int index)
		{
			if ( index == -1 )
			{//unload
				otpTimer.Stop();
				pd = null;
				OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
				return;
			}
			pd = vault.getViewWebsiteData(index);
			if (pd.OTP != "")
			{
				otpTimer = new Timer();
				otpTimer.Elapsed += new ElapsedEventHandler(OTPRefresh);
				otpTimer.Interval = 1000;
				otpTimer.Start();
			}
			blkThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(BreachTest));
			blkThread.IsBackground = true;
			blkThread.Start(index);

			OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
		}
        public ViewModel(string pass)
        {
			vault = new Model.Vault(pass);
			mini = vault.getMiniList();
			OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
		}

		public ViewWebsiteData Page
        {
            get { return pd; }
        }
        public List<MiniList> SavedList
        {
            get { return mini; }
        }
		public void Save()
		{
			vault.Serialize();
		}
		public void OTPRefresh(object source, ElapsedEventArgs e)
		{
			pd.Refresh();
		}
		public void BreachTest(object index)
		{
			if (Func.passLeak(vault.getPass((int)index)))
			{
				pd.Blacklisted = true; //pass leak
			}
			List<Dictionary<string, object>> breach = Func.BreachTest(pd.EMail);
			if(breach[0].ContainsKey("Leaked"))//not leaked
			{
				pd.Blacklisted = false; 
			}
			pd.Blacklisted = true; //account leak
		}

	}
}
