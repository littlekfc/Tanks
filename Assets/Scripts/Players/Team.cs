using UnityEngine;
using System.Collections;
using System;

namespace Tanks.Players
{
    public class Team
    {
        public enum TeamID
        {
            NONE,
            RED,
            BLUE
        }

        public TeamID ID { get; set; }

        public static Color GetColorFor(TeamID team)
        {
            switch (team)
            {
                case TeamID.BLUE:
                    return Color.blue;

                case TeamID.RED:
                    return Color.red;

                default:
                    return Color.white;
            }
        }
    }
}