using System;
using System.Linq;

namespace TeamProMobileApplicationIOS.Internals
{
    public enum StatusOkMode
    {
        [Name("")]
        [StatusOkDescriptionAttribute(StatusOkDescription.Undefined)]
        [StatusOkColorAttribute(ColorStatusMode.White)]
        Undefined,
        [Name("C")]
        [StatusOkDescriptionAttribute(StatusOkDescription.CommentInvalid)]
        [StatusOkColorAttribute(ColorStatusMode.Red)]
        CommentInvalid,
        [Name("D")]
        [StatusOkDescriptionAttribute(StatusOkDescription.InvalidDate)]
        [StatusOkColorAttribute(ColorStatusMode.Red)]
        InvalidDate,
        [Name("F")]
        [StatusOkDescriptionAttribute(StatusOkDescription.Forbidden)]
        [StatusOkColorAttribute(ColorStatusMode.Red)]
        Forbidden,
        [Name("I")]
        [StatusOkDescriptionAttribute(StatusOkDescription.InvalidIssue)]
        [StatusOkColorAttribute(ColorStatusMode.Red)]
        InvalidIssue,
        [Name("O")]
        [StatusOkDescriptionAttribute(StatusOkDescription.InvalidIwOrTtr)]
        [StatusOkColorAttribute(ColorStatusMode.Red)]
        InvalidIwOrTtr,
        [Name("P")]
        [StatusOkDescriptionAttribute(StatusOkDescription.ProjectInvalid)]
        [StatusOkColorAttribute(ColorStatusMode.Red)]
        ProjectInvalid,
        [Name("T")]
        [StatusOkDescriptionAttribute(StatusOkDescription.InvalidTime)]
        [StatusOkColorAttribute(ColorStatusMode.Red)]
        InvalidTime,
        [Name("U")]
        [StatusOkDescriptionAttribute(StatusOkDescription.InvalidUserRef)]
        [StatusOkColorAttribute(ColorStatusMode.Red)]
        InvalidUserRef,
        [Name("W")]
        [StatusOkDescriptionAttribute(StatusOkDescription.InvalidWorkType)]
        [StatusOkColorAttribute(ColorStatusMode.Red)]
        InvalidWorkType,
        [Name("Y")]
        [StatusOkDescriptionAttribute(StatusOkDescription.Valid)]
        [StatusOkColorAttribute(ColorStatusMode.Green)]
        Valid
    }

    public class StatusOkColorAttribute : StatusColorAttribute
    {
        public StatusOkColorAttribute(ColorStatusMode color)
            : base(color)
        { }
    }

    public class StatusOkDescriptionAttribute : Attribute
    {
        public StatusOkDescription Name { get; private set; }

        public StatusOkDescriptionAttribute(StatusOkDescription name)
        {
            Name = name;
        }
    }

    public static class StatusOkModeHelper
    {
        public static string GetColor(StatusOkMode status)
        {
            ColorStatusMode color = EnumHelper<StatusOkMode, StatusOkColorAttribute>.GetAttribute(status).Color;
            return ColorStatusModeHelper.GetName(color);
        }

        public static String GetCode(StatusOkMode status)
        {
            return status != StatusOkMode.Valid ? EnumHelper<StatusOkMode, NameAttribute>.GetAttribute(status).Name : String.Empty;
        }

        public static String GetName(StatusOkMode status)
        {
            StatusOkDescription description = EnumHelper<StatusOkMode, StatusOkDescriptionAttribute>.GetAttribute(status).Name;
            return StatusOkDescriptionHelper.GetDescription(description);
        }

        public static StatusOkMode FromName(string name)
        {
            return EnumHelper<StatusOkMode, NameAttribute>.GetValues().Where(s => s.Value.Name == name).Select(r => r.Key).FirstOrDefault();
        }
    }
}