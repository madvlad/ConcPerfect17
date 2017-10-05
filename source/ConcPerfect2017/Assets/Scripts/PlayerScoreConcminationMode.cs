using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PlayerScoreConcminationMode : IPlayerScore {
    private int capturedBeacons;
    public int CapturedBeacons {
        get {
            return capturedBeacons;
        }

        set {
            capturedBeacons = value;
        }
    }

    private PlayerInfo pInfo;
    public PlayerInfo PInfo
    {
        get
        {
            return pInfo;
        }

        set
        {
            pInfo = value;
        }
    }

    public int CompareTo(IPlayerScore that) {
        PlayerScoreConcminationMode thatCM = (PlayerScoreConcminationMode)that;
        int beaconsCompare = this.CapturedBeacons.CompareTo(thatCM.CapturedBeacons);
        return (beaconsCompare == 0) ? this.PInfo.Nickname.CompareTo(that.PInfo.Nickname) : beaconsCompare;
    }
}