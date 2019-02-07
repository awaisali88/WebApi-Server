using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NameAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ImageAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ImageListAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ImageUriAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SortColumnAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class LinkedEntityIdAttribute : Attribute
    {
        public string LinkedEntity { get; set; }

        public LinkedEntityIdAttribute(string linkedEntity)
        {
            LinkedEntity = linkedEntity;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class LinkedEntityCollectionAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CreatedByAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DateCreatedAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ModifiedByAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DateModifiedAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IsActiveAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RecordStatusAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OrganizationCodeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class LastLoginDateAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IsLoggedInAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UpdateEntityAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class DecimalPrecisionAttribute : Attribute
    {
        public byte Precision { get; set; }
        public byte Scale { get; set; }

        public DecimalPrecisionAttribute(byte precision, byte scale)
        {
            Precision = precision;
            Scale = scale;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class StringValueAttribute : Attribute
    {
        public StringValueAttribute(string stringValue)
        {
            Value = stringValue;
        }

        public string Value { get; set; }
    }
}
