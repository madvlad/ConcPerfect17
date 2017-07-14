using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeQualifier : MonoBehaviour {
    public float[] Level1Times;
    public float[] Level2Times;
    public float[] Level3Times;
    public float[] Level4Times;
    public float[] Level5Times;
	public float[] Level6Times;

	public int CheckTime(float time, int level)
    {
        switch(level)
        {
            case 1:
                return Place(time, Level1Times);
            case 2:
                return Place(time, Level2Times);
            case 3:
                return Place(time, Level3Times);
            case 4:
                return Place(time, Level4Times);
            case 5:
                return Place(time, Level5Times);
			case 6:
				return Place(time, Level6Times);
            default:
                return Place(time, Level1Times);
        }
    }

    /// <summary>
    /// This is running off the assumption that times will always be of length 4 and will be presorted by longest to shortest.
    /// 0 - Qualifier Time
    /// 1 - Bronze Time
    /// 2 - Silver Time
    /// 3 - Gold Time
    /// </summary>
    /// <param name="time">The time the player got</param>
    /// <param name="times">The list of the current level's times to beat</param>
    /// <returns>The number representation of their award</returns>
    private int Place(float time, float[] times)
    {
        int reward = -1;

        for (int i = 0; i < times.Length; i++)
        {
            if (time <= times[i])
            {
                reward = i;
            }
            else
            {
                break;
            }
        }

        return reward;
    }
}
