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
	[Table("active_directory", Schema = "dbo")]
    public partial class ActiveDirectory : Entity<int>
    {
        /* DO NOT UPDATE THIS FILE */

        #region Properties

        /// <summary>
        /// Property Name: ActiveDirectoryFriendlyName
        /// ColumnName: Active_Directory_Friendly_Name
        /// ColumnType: varchar
        /// </summary>
        [Column("Active_Directory_Friendly_Name", TypeName = "VARCHAR(500)"), SearchAble(Searchable.SearchAbleType.Contains), AutoComplete()]
        public string ActiveDirectoryFriendlyName { get; set; }

        /// <summary>
        /// Property Name: ActiveDirectoryID
        /// ColumnName: Active_Directory_ID
        /// ColumnType: int
        /// </summary>
        [Key(), Column("Active_Directory_ID")]
        public int ActiveDirectoryID { get; set; }

        /// <summary>
        /// Property Name: ActiveDirectoryLogin
        /// ColumnName: Active_Directory_Login
        /// ColumnType: varchar
        /// </summary>
        [Column("Active_Directory_Login", TypeName = "VARCHAR(500)"), SearchAble(Searchable.SearchAbleType.Contains), AutoComplete()]
        public string ActiveDirectoryLogin { get; set; }

        /// <summary>
        /// Property Name: CreateDate
        /// ColumnName: Create_Date
        /// ColumnType: datetime
        /// </summary>
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Property Name: EmailAddress
        /// ColumnName: Email_Address
        /// ColumnType: varchar
        /// </summary>
        [Column("Email_Address", TypeName = "VARCHAR(500)"), SearchAble(Searchable.SearchAbleType.Contains), AutoComplete()]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Property Name: ModifyDate
        /// ColumnName: Modify_Date
        /// ColumnType: datetime
        /// </summary>
        [Column("Modify_Date")]
        public DateTime? ModifyDate { get; set; }

        #endregion Properties

        /* DO NOT UPDATE THIS FILE */
    }
}