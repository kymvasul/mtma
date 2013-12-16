using System;

namespace TeamProMobileApplicationIOS.Internals
{
    public enum StatusOkDescription
    {
        [Name("")]
        Undefined,
        [Name("Comment invalid")]
        CommentInvalid,
        [Name("Invalid Date")]
        InvalidDate,
        [Name("Forbidden")]
        Forbidden,
        [Name("Invalid Issue")]
        InvalidIssue,
        [Name("Invalid IW or TTR")]
        InvalidIwOrTtr,
        [Name("Project Invalid")]
        ProjectInvalid,
        [Name("Invalid Time")]
        InvalidTime,
        [Name("Invalid UserRef")]
        InvalidUserRef,
        [Name("Invalid Work Type")]
        InvalidWorkType,
        [Name("Valid")]
        Valid
    }

    class StatusOkDescriptionHelper
    {
        public static string GetDescription(StatusOkDescription description)
        {
            return EnumHelper<StatusOkDescription, NameAttribute>.GetAttribute(description).Name;
        } 
    }
}
