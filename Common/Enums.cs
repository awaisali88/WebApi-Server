using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Common.JsonConverter;
using Newtonsoft.Json;

namespace Common
{
    public enum Gender
    {
        [Description("Male")]
        [StringValue("M")]
        Male = 1,

        [Description("Female")]
        [StringValue("F")]
        Female = 2,

        [Description("N/A")]
        [StringValue("N/A")]
        NA = 3
    }

    public enum Months
    {
        [StringValue("January")]
        January = 1,

        [StringValue("Febraury")]
        Febraury = 2,

        [StringValue("March")]
        March = 3,

        [StringValue("April")]
        April = 4,

        [StringValue("May")]
        May = 5,

        [StringValue("June")]
        June = 6,

        [StringValue("July")]
        July = 7,

        [StringValue("August")]
        August = 8,

        [StringValue("September")]
        September = 9,

        [StringValue("October")]
        October = 10,

        [StringValue("November")]
        November = 11,

        [StringValue("December")]
        December = 12,
    }

    public enum WeekDays
    {
        [StringValue("Monday")]
        Monday = 1,

        [StringValue("Tuesday")]
        Tuesday = 2,

        [StringValue("Wednesday")]
        Wednesday = 3,

        [StringValue("Thursday")]
        Thursday = 4,

        [StringValue("Friday")]
        Friday = 5,

        [StringValue("Saturday")]
        Saturday = 6,

        [StringValue("Sunday")]
        Sudnay = 7,
    }

    public enum RecurringPeriod
    {
        [StringValue("One Time")]
        [Description("Admisssion Fee, Registration Fee, Security Fee etc.")]
        OneTime = 1,

        [StringValue("Monthly")]
        [Description("Monthly Fee")]
        Monthly = 2,

        [StringValue("Every Two Months")]
        [Description("After Every Two Months")]
        BiMonthly = 3,

        [StringValue("Yearly")]
        [Description("Every Year")]
        Yearly = 4,
    }

    public enum PaymentTerms
    {
        [StringValue("Arrears")]
        [Description("After the end of term")]
        Arrears = 1,

        [StringValue("Advacne")]
        [Description("At the beginning of term")]
        Advance = 2,
    }

    public enum ContactType
    {
        [StringValue("Mobile No.")]
        [Description("Mobile No.")]
        Mobile = 1,

        [StringValue("Landline No.")]
        [Description("Landline No.")]
        Landline = 2,

        [StringValue("Email")]
        [Description("Email")]
        Email = 3,
    }

    public enum OrganizationType
    {
        [StringValue("Head Office")]
        [Description("Head Office")]
        HeadOffice = 1,

        [StringValue("Campus")]
        [Description("Campus")]
        Campus = 2,

        [StringValue("Franchise")]
        [Description("Franchise")]
        Franchise = 3,

        [StringValue("Branch")]
        [Description("Branch")]
        Branch = 3,
    }

    public enum StudentStatus
    {
        [StringValue("Active")]
        [Description("Active")]
        Active = 1,

        [StringValue("Left")]
        [Description("Left")]
        Left = 2,

        [StringValue("Unknown")]
        [Description("Unknown")]
        Unknown = 3,

        [StringValue("StruckOff")]
        [Description("StruckOff")]
        StruckOff = 3,
    }

    // ReSharper disable InconsistentNaming
    /// <summary>
    ///     Type Sql provider
    /// </summary>
    public enum SqlProvider
    {
        /// <summary>
        ///     MSSQL
        /// </summary>
        MSSQL,

        /// <summary>
        ///     MySQL
        /// </summary>
        MySQL,

        /// <summary>
        ///     PostgreSQL
        /// </summary>
        PostgreSQL
    }

    public enum RecordStatus
    {
        [StringValue("Domain Mode")]
        [Description("Domain Data")]
        DomainMode = 1,

        [StringValue("New Mode")]
        [Description("New/Add Mode (Insertion for Records, Store intRecStatus value to mark the record as new insertion)")]
        NewMode = 2,

        [StringValue("Edit Mode")]
        [Description("Edit/Modify Mode (Updation for Records, Store intRecStatus value to mark the record as modified/updated)")]
        EditMode = 3,

        [StringValue("ReActivate Old Mode")]
        [Description("RecActivate Deleted Records with old enteries, dont store intRecStatus value)")]
        ReActivateOldMode = 4,

        [StringValue("ReActivate New Mode")]
        [Description("Hard delete old or deleted record and insert New Data instead of old, dont store intRecStatus value")]
        ReActivateNewMode = 5,

        [StringValue("Record Check Mode")]
        [Description("For Checking/Loading non deleted Data withthe help of Where Clause, dont store intRecStatus value")]
        RecordCheckMode = 6,

        [StringValue("Load Data Mode")]
        [Description("For Checking/Loading the Whole Data including deleted data, dont store intRecStatus value)")]
        LoadDataMode = 7,

        [StringValue("Dublicate Check for Edit Mode")]
        [Description("For Checking Dublication in all data")]
        DublicateCheckforEditMode = 8,

        [StringValue("Only Activate Record Mode")]
        [Description("Load Only Active and non deleted Records, dont store intRecStatus value")]
        OnlyActivateRecordMode = 9,

        [StringValue("Editing")]
        [Description("Inidcating a record has been modified by user in its current transaction")]
        Editing = 10,

        [StringValue("Reverse Mode")]
        [Description("Reverse Mode (Reverse for Records, Store intRecStatus value to mark the record as Reverse)")]
        ReverseMode = 11,

        [StringValue("Void Mode")]
        [Description("Inidcating a record needs to void")]
        VoidMode = 12,

        [StringValue("Delete Mode")]
        [Description("Delete Mode (Deletion for Records, Store intRecStatus value to mark the record as soft delete)")]
        DeleteMode = 13,

        [StringValue("Closed")]
        [Description("Delete Mode (Deletion for Records, Store intRecStatus value to mark the record as soft delete)")]
        Closed = 14,

        [StringValue("Physical Delete")]
        [Description("Inidcating a record needs to be deleted physcially")]
        PhysicalDelete = 15,

        [StringValue("Cancel")]
        [Description("Cancel")]
        Cancel = 16
    }
}
