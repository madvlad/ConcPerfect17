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
    public int CourseJumpLimit;
    public string BestTime;
    public int TimesCompleted;
    public int PlayerModel;
    public int BeaconsCaptured;
    public string CurrentTeam = null;

    public int CompareTo(PlayerInfo that) {
        if (ApplicationManager.GameType == GameTypes.ConcminationGameType) {
            // Sort By Team First

            int teamCompare =  this.CurrentTeam.CompareTo(that.CurrentTeam);
            if (teamCompare == 0) {
                // Sort by capture count second
                int beaconsCompare = this.BeaconsCaptured.CompareTo(that.BeaconsCaptured);

                return (beaconsCompare == 0) ? this.Nickname.CompareTo(that.Nickname) : (beaconsCompare);
            } else {
                return teamCompare;
            }
        } else {
            int timeCompare = this.BestTime.CompareTo(that.BestTime);

            return (timeCompare == 0) ? this.Nickname.CompareTo(that.Nickname) : (timeCompare);
        }
    }

    public string PrintPlayerInfoRaceMode() {
        return this.PlayerId.ToString() + " ; " + this.Nickname + " ; " + this.Status + " ; " + this.CurrentJump + "/" + this.CourseJumpLimit + " ; " + this.BestTime + " ; " + this.TimesCompleted + " % ";
    }

    public string PrintPlayerInfoConcminationMode() {
        return this.PlayerId.ToString() + " ; " + this.Nickname + " ; " + this.Status + " ; " + this.BeaconsCaptured+ " % ";
    }
}
