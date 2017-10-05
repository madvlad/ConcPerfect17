using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Networking;

public class PlayerInfo {
    public NetworkInstanceId PlayerId;
    public string Nickname;
    public string Status;
    public int CurrentJump;
    public string BestTime;
    public int TimesCompleted;
    public int PlayerModel;
    public int BeaconsCaptured;
    public string CurrentTeam = "";

    public int CompareTo(PlayerInfo that) {
        int timeCompare = this.BestTime.CompareTo(that.BestTime);
        int beaconsCompare = this.BeaconsCaptured.CompareTo(that.BeaconsCaptured);

        return ((timeCompare|beaconsCompare) == 0 ) ? this.Nickname.CompareTo(that.Nickname) : (timeCompare|beaconsCompare);
    }
}
