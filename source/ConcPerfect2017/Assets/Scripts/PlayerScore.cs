using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IPlayerScore {
    PlayerInfo PInfo {
        get;
        set;
    }

    int CompareTo(IPlayerScore that);
}