using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Signals.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Signals.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SyncController : ControllerBase
	{
		private readonly ILogger<SyncController> _logger;

		public SyncController(ILogger<SyncController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		public JsonResult Post(RequestData data)
		{
            try
            {
                List<Track> tracks = data.Data.Select(x => x.Track).GroupBy(x => x.Idtrack).Select(x => x.First()).ToList();
                string connStr = "server=213.190.6.89;user=u223000638_signals;database=u223000638_signals;port=3306;password=Kohala2112;SSL Mode=None";
                MySqlConnection conn = new(connStr);
                conn.Open();
                MySqlCommand cmd = new("DELETE FROM reading", conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand("DELETE FROM track", conn);
                cmd.ExecuteNonQuery();

                foreach (Track t in tracks)
                {
                    cmd = new MySqlCommand("INSERT INTO track (idtrack, start, end) VALUES (" + t.Idtrack + ", '" + t.Start.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + t.End.ToString("yyyy-MM-dd HH:mm:ss") + "')", conn);
                    cmd.ExecuteNonQuery();
                }

                foreach (Reading r in data.Data)
                {
                    int categoria = 0;

                    if(r.Rsrp >= -80 && r.Rsrq >= -10 && r.Rssnr >= 20)
					{
                        categoria = 1;
					}
                    if (r.Rsrp >= -90 && r.Rsrp < -80 && r.Rsrq >= -15 && r.Rsrq < -10 && r.Rssnr >= 13 && r.Rssnr < 20)
                    {
                        categoria = 2;
                    }
                    if (r.Rsrp >= -100 && r.Rsrp < -90 && r.Rsrq >= -20 && r.Rsrq < -15 && r.Rssnr >= 0 && r.Rssnr < 13)
                    {
                        categoria = 3;
                    }
                    if (r.Rsrp < -100 && r.Rsrq < -20 && r.Rssnr < 0)
                    {
                        categoria = 4;
                    }

                    cmd = new MySqlCommand("INSERT INTO reading "
                        + "(idreading, idtrack, date, asuLevel, cqi, "
                        + "dbm, level, rsrp, rsrq, rssi, "
                        + "rssnr, timingAdvance, bandwidth, ci, earfcn, "
                        + "mcc, mnc, operator, pci, tac, "
                        + "latitude, longitude, categoria) "
                        + "VALUES "
                        + "(" + r.Idreading + ", " + r.Track.Idtrack + ", '" + r.Date.ToString("yyyy-MM-dd HH:mm:ss") + "', " + r.Asulevel + ", " + r.Cqi + ", "
                        + r.Dbm + ", " + r.Level + ", " + r.Rsrp + ", " + r.Rsrq + ", " + r.Rssi + ", "
                        + r.Rssnr + ", " + r.TimingAdvance + ", " + r.Bandwidth + ", " + r.Ci + ", " + r.Earfcn + ", "
                        + r.Mcc + ", " + r.Mnc + ", " + r.Operator + ", " + r.Pci + ", " + r.Tac + ", "
                        + r.Latitude.ToString(CultureInfo.InvariantCulture) + ", " + r.Longitude.ToString(CultureInfo.InvariantCulture) + ", " + categoria + ")"
                        , conn);
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
                return new JsonResult(new { result = "Ok" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { result = "Error: " + ex.StackTrace });
            }
            
		}

        [HttpGet]
        public string Get()
		{
            return "Servicio corriendo";
		}
	}
}
