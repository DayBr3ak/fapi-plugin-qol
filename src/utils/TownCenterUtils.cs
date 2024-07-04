
using System;

public class TownCenterUtils {
    public static readonly double MAX_DEAL_TIME = 10800.0;
    static public double GetMaxDealTime() {
        var PD = GameManager.i.PD;
        return MAX_DEAL_TIME * (1.0 - PD.ExpeShopTradeCenterFasterChargeLevel * 0.02);
    }

    static public double GetDealTimeUntilFull() {
        var PD = GameManager.i.PD;
        var remainingCharges = PD.TradeCenterMaxCharge - PD.TradeCenterCurrentCharge;
        if (remainingCharges <= 0) {
            return 0.0;
        }
        return PD.TradeCenterDealTime + (remainingCharges - 1) * GetMaxDealTime();
    }

    static public string FormatDealTime(double Timeleft) {
        // Ripped from the decompiled game code
        string text;
        var GM = GameManager.i;
		if (Timeleft >= 172800.0)
		{
			text = string.Concat(new string[]
			{
				Math.Floor(Timeleft / 86400.0 % 10000.0).ToString("00"),
				" ",
				GM.TOT.Days,
				" ",
				Math.Floor(Timeleft / 3600.0 % 24.0).ToString("00"),
				":",
				Math.Floor(Timeleft / 60.0 % 60.0).ToString("00"),
				":",
				Math.Floor(Timeleft % 60.0).ToString("00")
			});
		}
		else if (Timeleft >= 86400.0)
		{
			text = string.Concat(new string[]
			{
				Math.Floor(Timeleft / 86400.0 % 100.0).ToString("00"),
				" ",
				GM.TOT.Day,
				" ",
				Math.Floor(Timeleft / 3600.0 % 24.0).ToString("00"),
				":",
				Math.Floor(Timeleft / 60.0 % 60.0).ToString("00"),
				":",
				Math.Floor(Timeleft % 60.0).ToString("00")
			});
		}
		else if (Timeleft >= 3600.0)
		{
			text = string.Concat(new string[]
			{
				Math.Floor(Timeleft / 3600.0 % 24.0).ToString("00"),
				":",
				Math.Floor(Timeleft / 60.0 % 60.0).ToString("00"),
				":",
				Math.Floor(Timeleft % 60.0).ToString("00")
			});
		}
		else if (Timeleft >= 60.0)
		{
			text = Math.Floor(Timeleft / 60.0 % 60.0).ToString("00") + ":" + Math.Floor(Timeleft % 60.0).ToString("00");
		}
		else if (Timeleft >= 0.0)
		{
			text = Math.Floor(Timeleft % 60.0).ToString("N0");
		}
		else
		{
			text = "--:--";
		}
		return text;
    }
}