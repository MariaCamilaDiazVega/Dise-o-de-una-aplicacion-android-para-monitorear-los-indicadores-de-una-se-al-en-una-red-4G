using System;

namespace Signals.Models
{
	public class Reading
	{
		public int Idreading { get; set; }

		public Track Track { get; set; }

		public DateTime Date { get; set; }

		public int Asulevel { get; set; }

		public int Cqi { get; set; }
		
		public int Dbm { get; set; }

		public int Level { get; set; }
		
		public int Rsrp { get; set; }

		public int Rsrq { get; set; }

		public int Rssi { get; set; }
		
		public int Rssnr { get; set; }
		
		public int TimingAdvance { get; set; }
		
		public int Bandwidth { get; set; }
		
		public int Ci { get; set; }
		
		public int Earfcn { get; set; }
		
		public int Mcc { get; set; }
		
		public int Mnc { get; set; }
		
		public int Operator { get; set; }
		
		public int Pci { get; set; }
		
		public int Tac { get; set; }
		
		public float Latitude { get; set; }
		
		public float Longitude { get; set; }

	}
}