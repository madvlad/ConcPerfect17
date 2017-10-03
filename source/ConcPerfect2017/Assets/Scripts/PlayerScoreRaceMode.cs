using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlayerScoreRaceMode : IPlayerScore {
    public string CurrentTimerTime;
    private PlayerInfo pInfo;

    public PlayerInfo PInfo {
        get {
            return pInfo;
        }

        set {
            pInfo = value;
        }
    }

    public int CompareTo(IPlayerScore that) {
        PlayerScoreRaceMode thatRM = (PlayerScoreRaceMode) that;
        int timeCompare = this.CurrentTimerTime.CompareTo(thatRM.CurrentTimerTime);
        return (timeCompare == 0) ? this.PInfo.Nickname.CompareTo(that.PInfo.Nickname) : timeCompare;
    }
}
