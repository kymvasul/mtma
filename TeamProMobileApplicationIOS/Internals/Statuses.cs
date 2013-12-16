using System;
using System.Linq;

namespace TeamProMobileApplicationIOS.Internals
{
    public enum StatusStMode
    {
        [Name("")]
        [StatusColor(ColorStatusMode.White)]
        Undefined,
        [Name("Active")]
        [StatusColor(ColorStatusMode.LightSteelBlue)]
        Active,
        [Name("Submitted")]
        [StatusColor(ColorStatusMode.Yellow)]
        Submittet,
        [Name("Rejected")]
        [StatusColor(ColorStatusMode.Red)]
        Rejected,
        [Name("Re-submitted")]
        [StatusColor(ColorStatusMode.Magenta)]
        ReSubmittet,
        [Name("Confirmed")]
        [StatusColor(ColorStatusMode.Lime)]
        Confirmed,
        [Name("Approved")]
        [StatusColor(ColorStatusMode.Green)]
        Approved,
        [Name("Closed")]
        [StatusColor(ColorStatusMode.Green)]
        Closed
    }
    
    public class StatusColorAttribute : Attribute
    {
        public ColorStatusMode Color { get; private set; }

        public StatusColorAttribute(ColorStatusMode color)
        {
            Color = color;
        }
    }

    public static class StatusStModeHelper
    {
        public static String GetColor(StatusStMode status)
        {
            ColorStatusMode color = EnumHelper<StatusStMode, StatusColorAttribute>.GetAttribute(status).Color;
            return ColorStatusModeHelper.GetName(color);
        }

        public static String GetName(StatusStMode status)
        {
            return EnumHelper<StatusStMode, NameAttribute>.GetAttribute(status).Name;
        }

        public static StatusStMode FromName(string name)
        {
            return EnumHelper<StatusStMode, NameAttribute>.GetValues().Where(s => s.Value.Name == name).Select(r => r.Key).FirstOrDefault();
        }
    }
}
