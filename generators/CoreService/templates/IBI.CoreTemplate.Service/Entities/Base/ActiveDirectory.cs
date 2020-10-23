using IBI.<%= Name %>.Service.Core.Attributes;
using IBI.<%= Name %>.Service.Core.Entities;
using IBI.<%= Name %>.Service.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static IBI.<%= Name %>.Service.Core.Attributes.Searchable;

namespace IBI.<%= Name %>.Service.Core.Context
{
    /// <summary>
    /// Adds ActiveDirectory to the set of tables the main context can use
    /// </summary>
    public partial class MainContext : DbContext
    {
        #region Properties

        internal DbSet<ActiveDirectory> ActiveDirectory { get; set; }

        #endregion Properties
    }
}

namespace IBI.<%= Name %>.Service.Entities
{
    /// <summary>
	/// Entity for the active_directory table
	/// </summary>
	[Table("active_directory", Schema = "dbo"), ReadOnly()]
    public partial class ActiveDirectory : Entity<int>
    {
        /* DO NOT UPDATE THIS FILE */

        #region Properties
		
		
		/// <summary>
		/// Property Name: ActiveDirectoryID
		/// ColumnName: Active_Directory_ID
		/// ColumnType: int
		/// </summary>
		[Key(), Column("Active_Directory_ID")]
		public int ActiveDirectoryID { get; set; }
		
		/// <summary>
		/// Property Name: CreateDate
		/// ColumnName: Create_Date
		/// ColumnType: datetime
		/// </summary>
		[Column("Create_Date")]
		public DateTime? CreateDate { get; set; }
		
		/// <summary>
		/// Property Name: ModifyDate
		/// ColumnName: Modify_Date
		/// ColumnType: datetime
		/// </summary>
		[Column("Modify_Date")]
		public DateTime? ModifyDate { get; set; }
		
		/// <summary>
		/// Property Name: ActiveDirectoryLogin
		/// ColumnName: Active_Directory_Login
		/// ColumnType: varchar
		/// </summary>
		[Column("Active_Directory_Login", TypeName = "VARCHAR(500)" ), StringLength(500), MaxLength(500)]
		public string ActiveDirectoryLogin { get; set; }
		
		/// <summary>
		/// Property Name: ActiveDirectoryFriendlyName
		/// ColumnName: Active_Directory_Friendly_Name
		/// ColumnType: varchar
		/// </summary>
		[Column("Active_Directory_Friendly_Name", TypeName = "VARCHAR(500)" ), StringLength(500), MaxLength(500)]
		public string ActiveDirectoryFriendlyName { get; set; }
		
		/// <summary>
		/// Property Name: EmailAddress
		/// ColumnName: Email_Address
		/// ColumnType: varchar
		/// </summary>
		[Column("Email_Address", TypeName = "VARCHAR(500)" ), StringLength(500), MaxLength(500)]
		public string EmailAddress { get; set; }
		
		/// <summary>
		/// Property Name: PrimaryActiveDirectorySourceId
		/// ColumnName: PrimaryActiveDirectorySourceId
		/// ColumnType: int
		/// </summary>
		[Column("PrimaryActiveDirectorySourceId")]
		public int? PrimaryActiveDirectorySourceId { get; set; }
		
		/// <summary>
		/// Property Name: PrimaryLogin
		/// ColumnName: PrimaryLogin
		/// ColumnType: varchar
		/// </summary>
		[Column("PrimaryLogin", TypeName = "VARCHAR(500)" ), StringLength(500), MaxLength(500)]
		public string PrimaryLogin { get; set; }
		
		/// <summary>
		/// Property Name: PrimaryLogicallyDeleted
		/// ColumnName: PrimaryLogicallyDeleted
		/// ColumnType: bit
		/// </summary>
		[Column("PrimaryLogicallyDeleted")]
		public bool? PrimaryLogicallyDeleted { get; set; }
		
		/// <summary>
		/// Property Name: AlternateActiveDirectorySourceId
		/// ColumnName: AlternateActiveDirectorySourceId
		/// ColumnType: int
		/// </summary>
		[Column("AlternateActiveDirectorySourceId")]
		public int? AlternateActiveDirectorySourceId { get; set; }
		
		/// <summary>
		/// Property Name: AlternateActiveDirectoryLogin
		/// ColumnName: AlternateActiveDirectoryLogin
		/// ColumnType: varchar
		/// </summary>
		[Column("AlternateActiveDirectoryLogin", TypeName = "VARCHAR(500)" ), StringLength(500), MaxLength(500)]
		public string AlternateActiveDirectoryLogin { get; set; }
		
		/// <summary>
		/// Property Name: AlternateLogin
		/// ColumnName: AlternateLogin
		/// ColumnType: varchar
		/// </summary>
		[Column("AlternateLogin", TypeName = "VARCHAR(500)" ), StringLength(500), MaxLength(500)]
		public string AlternateLogin { get; set; }
		
		/// <summary>
		/// Property Name: AlternateActiveDirectoryFriendlyName
		/// ColumnName: AlternateActiveDirectoryFriendlyName
		/// ColumnType: varchar
		/// </summary>
		[Column("AlternateActiveDirectoryFriendlyName", TypeName = "VARCHAR(500)" ), StringLength(500), MaxLength(500)]
		public string AlternateActiveDirectoryFriendlyName { get; set; }
		
		/// <summary>
		/// Property Name: AlternateEmailAddress
		/// ColumnName: AlternateEmailAddress
		/// ColumnType: varchar
		/// </summary>
		[Column("AlternateEmailAddress", TypeName = "VARCHAR(500)" ), StringLength(500), MaxLength(500)]
		public string AlternateEmailAddress { get; set; }
		
		/// <summary>
		/// Property Name: AlternateLogicallyDeleted
		/// ColumnName: AlternateLogicallyDeleted
		/// ColumnType: bit
		/// </summary>
		[Column("AlternateLogicallyDeleted")]
		public bool? AlternateLogicallyDeleted { get; set; }
		
		/// <summary>
		/// Property Name: ParentActiveDirectoryId
		/// ColumnName: ParentActiveDirectoryId
		/// ColumnType: int
		/// </summary>
		[Column("ParentActiveDirectoryId")]
		public int? ParentActiveDirectoryId { get; set; }
		
		/// <summary>
		/// Property Name: ReportingGroupId
		/// ColumnName: ReportingGroupId
		/// ColumnType: int
		/// </summary>
		[Column("ReportingGroupId")]
		public int? ReportingGroupId { get; set; }
		
		/// <summary>
		/// Property Name: CreatedByUserId
		/// ColumnName: CreatedByUserId
		/// ColumnType: int
		/// </summary>
		[Column("CreatedByUserId")]
		public int? CreatedByUserId { get; set; }
		
		/// <summary>
		/// Property Name: CreatedDateTime
		/// ColumnName: CreatedDateTime
		/// ColumnType: datetime
		/// </summary>
		[Column("CreatedDateTime")]
		public DateTime? CreatedDateTime { get; set; }
		
		/// <summary>
		/// Property Name: UpdatedByUserId
		/// ColumnName: UpdatedByUserId
		/// ColumnType: int
		/// </summary>
		[Column("UpdatedByUserId")]
		public int? UpdatedByUserId { get; set; }
		
		/// <summary>
		/// Property Name: UpdatedDatetime
		/// ColumnName: UpdatedDatetime
		/// ColumnType: datetime
		/// </summary>
		[Column("UpdatedDatetime")]
		public DateTime? UpdatedDatetime { get; set; }
		
		/// <summary>
		/// Property Name: ActiveDirectoryBusinessUnitId
		/// ColumnName: ActiveDirectoryBusinessUnitId
		/// ColumnType: int
		/// </summary>
		[Column("ActiveDirectoryBusinessUnitId")]
		public int? ActiveDirectoryBusinessUnitId { get; set; }
		
		
		#endregion Properties

        /* DO NOT UPDATE THIS FILE */
    }
}