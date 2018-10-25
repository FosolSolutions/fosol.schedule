using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fosol.Schedule.DAL.Migrations
{
    public partial class v010000 : DataMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreDeploy(migrationBuilder);

            migrationBuilder.CreateTable(
                name: "UserAccountRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    AccountRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccountRoles", x => new { x.UserId, x.AccountRoleId });
                });

            migrationBuilder.CreateTable(
                name: "AccountRoles",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    Privileges = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountUsers",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountUsers", x => new { x.AccountId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    Key = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 250, nullable: false),
                    State = table.Column<int>(nullable: false),
                    AddedById = table.Column<int>(nullable: true),
                    DefaultAccountId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    IsPrimary = table.Column<bool>(nullable: false),
                    Address1 = table.Column<string>(maxLength: 150, nullable: false),
                    Address2 = table.Column<string>(maxLength: 150, nullable: true),
                    City = table.Column<string>(maxLength: 150, nullable: false),
                    Province = table.Column<string>(maxLength: 150, nullable: false),
                    PostalCode = table.Column<string>(maxLength: 20, nullable: false),
                    Country = table.Column<string>(maxLength: 100, nullable: false),
                    Category = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 100, nullable: false),
                    Value = table.Column<string>(maxLength: 100, nullable: false),
                    ValueType = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attributes_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attributes_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactInfo",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Category = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInfo_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactInfo_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Criteria",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Criteria = table.Column<string>(nullable: false),
                    IsGroup = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Criteria_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Criteria_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    CalendarId = table.Column<int>(nullable: false),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 250, nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 100, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    Gender = table.Column<int>(nullable: true),
                    Birthdate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participants_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participants_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    State = table.Column<int>(nullable: false),
                    AddedById = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => new { x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_Tags_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tags_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Birthdate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserInfo_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInfo_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserInfo_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 50, nullable: false),
                    Value = table.Column<string>(maxLength: 500, nullable: false),
                    ValueType = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddresses",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    AddressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddresses", x => new { x.UserId, x.AddressId });
                    table.ForeignKey(
                        name: "FK_UserAddresses_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAddresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CalendarAttributes",
                columns: table => new
                {
                    CalendarId = table.Column<int>(nullable: false),
                    AttributeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarAttributes", x => new { x.CalendarId, x.AttributeId });
                    table.ForeignKey(
                        name: "FK_CalendarAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalendarAttributes_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAttributes",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    AttributeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAttributes", x => new { x.UserId, x.AttributeId });
                    table.ForeignKey(
                        name: "FK_UserAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAttributes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContactInfo",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ContactInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContactInfo", x => new { x.UserId, x.ContactInfoId });
                    table.ForeignKey(
                        name: "FK_UserContactInfo_ContactInfo_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "ContactInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContactInfo_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CalendarCriteria",
                columns: table => new
                {
                    CalendarId = table.Column<int>(nullable: false),
                    CriteriaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarCriteria", x => new { x.CalendarId, x.CriteriaId });
                    table.ForeignKey(
                        name: "FK_CalendarCriteria_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalendarCriteria_Criteria_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "Criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantAddresses",
                columns: table => new
                {
                    ParticipantId = table.Column<int>(nullable: false),
                    AddressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantAddresses", x => new { x.ParticipantId, x.AddressId });
                    table.ForeignKey(
                        name: "FK_ParticipantAddresses_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipantAddresses_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantAttributes",
                columns: table => new
                {
                    ParticipantId = table.Column<int>(nullable: false),
                    AttributeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantAttributes", x => new { x.ParticipantId, x.AttributeId });
                    table.ForeignKey(
                        name: "FK_ParticipantAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipantAttributes_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantContactInfo",
                columns: table => new
                {
                    ParticipantId = table.Column<int>(nullable: false),
                    ContactInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantContactInfo", x => new { x.ParticipantId, x.ContactInfoId });
                    table.ForeignKey(
                        name: "FK_ParticipantContactInfo_ContactInfo_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "ContactInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipantContactInfo_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<Guid>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    SubscriptionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CalendarId = table.Column<int>(nullable: false),
                    Key = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    StartOn = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    EndOn = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    State = table.Column<int>(nullable: false),
                    TagKey = table.Column<string>(nullable: true),
                    TagValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Tags_TagKey_TagValue",
                        columns: x => new { x.TagKey, x.TagValue },
                        principalTable: "Tags",
                        principalColumns: new[] { "Key", "Value" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventId = table.Column<int>(nullable: false),
                    Key = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    StartOn = table.Column<DateTime>(nullable: true),
                    EndOn = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activities_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activities_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventCriteria",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    CriteriaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCriteria", x => new { x.EventId, x.CriteriaId });
                    table.ForeignKey(
                        name: "FK_EventCriteria_Criteria_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "Criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventCriteria_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventTags",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    TagKey = table.Column<string>(nullable: false),
                    TagValue = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTags", x => new { x.EventId, x.TagKey, x.TagValue });
                    table.ForeignKey(
                        name: "FK_EventTags_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventTags_Tags_TagKey_TagValue",
                        columns: x => new { x.TagKey, x.TagValue },
                        principalTable: "Tags",
                        principalColumns: new[] { "Key", "Value" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityCriteria",
                columns: table => new
                {
                    ActivityId = table.Column<int>(nullable: false),
                    CriteriaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityCriteria", x => new { x.ActivityId, x.CriteriaId });
                    table.ForeignKey(
                        name: "FK_ActivityCriteria_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityCriteria_Criteria_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "Criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Openings",
                columns: table => new
                {
                    AddedById = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActivityId = table.Column<int>(nullable: false),
                    Key = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    MinParticipants = table.Column<int>(nullable: false),
                    MaxParticipants = table.Column<int>(nullable: false),
                    OpeningType = table.Column<int>(nullable: false),
                    ApplicationProcess = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Openings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Openings_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Openings_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Openings_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpeningCriteria",
                columns: table => new
                {
                    OpeningId = table.Column<int>(nullable: false),
                    CriteriaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningCriteria", x => new { x.OpeningId, x.CriteriaId });
                    table.ForeignKey(
                        name: "FK_OpeningCriteria_Criteria_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "Criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpeningCriteria_Openings_OpeningId",
                        column: x => x.OpeningId,
                        principalTable: "Openings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpeningParticipantApplications",
                columns: table => new
                {
                    OpeningId = table.Column<int>(nullable: false),
                    ParticipantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningParticipantApplications", x => new { x.OpeningId, x.ParticipantId });
                    table.ForeignKey(
                        name: "FK_OpeningParticipantApplications_Openings_OpeningId",
                        column: x => x.OpeningId,
                        principalTable: "Openings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OpeningParticipantApplications_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpeningParticipants",
                columns: table => new
                {
                    OpeningId = table.Column<int>(nullable: false),
                    ParticipantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningParticipants", x => new { x.OpeningId, x.ParticipantId });
                    table.ForeignKey(
                        name: "FK_OpeningParticipants_Openings_OpeningId",
                        column: x => x.OpeningId,
                        principalTable: "Openings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OpeningParticipants_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountRoles_AccountId",
                table: "AccountRoles",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRoles_AddedById",
                table: "AccountRoles",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRoles_UpdatedById",
                table: "AccountRoles",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AddedById",
                table: "Accounts",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Key",
                table: "Accounts",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_SubscriptionId",
                table: "Accounts",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UpdatedById",
                table: "Accounts",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_OwnerId_State",
                table: "Accounts",
                columns: new[] { "OwnerId", "State" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountUsers_UserId",
                table: "AccountUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_AddedById",
                table: "Activities",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_Key",
                table: "Activities",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UpdatedById",
                table: "Activities",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_EventId_State_StartOn_EndOn_Name",
                table: "Activities",
                columns: new[] { "EventId", "State", "StartOn", "EndOn", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCriteria_CriteriaId",
                table: "ActivityCriteria",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AddedById",
                table: "Addresses",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UpdatedById",
                table: "Addresses",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Name_IsPrimary_Province_PostalCode_Country",
                table: "Addresses",
                columns: new[] { "Name", "IsPrimary", "Province", "PostalCode", "Country" });

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_AddedById",
                table: "Attributes",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_UpdatedById",
                table: "Attributes",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_Key_Value",
                table: "Attributes",
                columns: new[] { "Key", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarAttributes_AttributeId",
                table: "CalendarAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarCriteria_CriteriaId",
                table: "CalendarCriteria",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_AccountId",
                table: "Calendars",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_AddedById",
                table: "Calendars",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_Key",
                table: "Calendars",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_UpdatedById",
                table: "Calendars",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_Name_State",
                table: "Calendars",
                columns: new[] { "Name", "State" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_AddedById",
                table: "ContactInfo",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_UpdatedById",
                table: "ContactInfo",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_Name_Category_Value",
                table: "ContactInfo",
                columns: new[] { "Name", "Category", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_AddedById",
                table: "Criteria",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_UpdatedById",
                table: "Criteria",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EventCriteria_CriteriaId",
                table: "EventCriteria",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_AddedById",
                table: "Events",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Key",
                table: "Events",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_UpdatedById",
                table: "Events",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Events_TagKey_TagValue",
                table: "Events",
                columns: new[] { "TagKey", "TagValue" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CalendarId_State_StartOn_EndOn_Name",
                table: "Events",
                columns: new[] { "CalendarId", "State", "StartOn", "EndOn", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_EventTags_TagKey_TagValue",
                table: "EventTags",
                columns: new[] { "TagKey", "TagValue" });

            migrationBuilder.CreateIndex(
                name: "IX_OpeningCriteria_CriteriaId",
                table: "OpeningCriteria",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_OpeningParticipantApplications_ParticipantId",
                table: "OpeningParticipantApplications",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_OpeningParticipants_ParticipantId",
                table: "OpeningParticipants",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Openings_AddedById",
                table: "Openings",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Openings_Key",
                table: "Openings",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Openings_UpdatedById",
                table: "Openings",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Openings_ActivityId_State_OpeningType_ApplicationProcess_Name",
                table: "Openings",
                columns: new[] { "ActivityId", "State", "OpeningType", "ApplicationProcess", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantAddresses_AddressId",
                table: "ParticipantAddresses",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantAttributes_AttributeId",
                table: "ParticipantAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantContactInfo_ContactInfoId",
                table: "ParticipantContactInfo",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_AddedById",
                table: "Participants",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_CalendarId",
                table: "Participants",
                column: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_Key",
                table: "Participants",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_UpdatedById",
                table: "Participants",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_UserId",
                table: "Participants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_Email_State",
                table: "Participants",
                columns: new[] { "Email", "State" });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_AddedById",
                table: "Subscriptions",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Key",
                table: "Subscriptions",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UpdatedById",
                table: "Subscriptions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Name_State",
                table: "Subscriptions",
                columns: new[] { "Name", "State" });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_AddedById",
                table: "Tags",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UpdatedById",
                table: "Tags",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountRoles_AccountRoleId",
                table: "UserAccountRoles",
                column: "AccountRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_AddressId",
                table: "UserAddresses",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAttributes_AttributeId",
                table: "UserAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContactInfo_ContactInfoId",
                table: "UserContactInfo",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_AddedById",
                table: "UserInfo",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_UpdatedById",
                table: "UserInfo",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddedById",
                table: "Users",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DefaultAccountId",
                table: "Users",
                column: "DefaultAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Key",
                table: "Users",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_State",
                table: "Users",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedById",
                table: "Users",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_AddedById",
                table: "UserSettings",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UpdatedById",
                table: "UserSettings",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId_Key",
                table: "UserSettings",
                columns: new[] { "UserId", "Key" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountRoles_Users_UserId",
                table: "UserAccountRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountRoles_AccountRoles_AccountRoleId",
                table: "UserAccountRoles",
                column: "AccountRoleId",
                principalTable: "AccountRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRoles_Accounts_AccountId",
                table: "AccountRoles",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRoles_Users_AddedById",
                table: "AccountRoles",
                column: "AddedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRoles_Users_UpdatedById",
                table: "AccountRoles",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountUsers_Accounts_AccountId",
                table: "AccountUsers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountUsers_Users_UserId",
                table: "AccountUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Calendars_Accounts_AccountId",
                table: "Calendars",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Calendars_Users_AddedById",
                table: "Calendars",
                column: "AddedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Calendars_Users_UpdatedById",
                table: "Calendars",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Accounts_DefaultAccountId",
                table: "Users",
                column: "DefaultAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            PostDeploy(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Accounts_DefaultAccountId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AccountUsers");

            migrationBuilder.DropTable(
                name: "ActivityCriteria");

            migrationBuilder.DropTable(
                name: "CalendarAttributes");

            migrationBuilder.DropTable(
                name: "CalendarCriteria");

            migrationBuilder.DropTable(
                name: "EventCriteria");

            migrationBuilder.DropTable(
                name: "EventTags");

            migrationBuilder.DropTable(
                name: "OpeningCriteria");

            migrationBuilder.DropTable(
                name: "OpeningParticipantApplications");

            migrationBuilder.DropTable(
                name: "OpeningParticipants");

            migrationBuilder.DropTable(
                name: "ParticipantAddresses");

            migrationBuilder.DropTable(
                name: "ParticipantAttributes");

            migrationBuilder.DropTable(
                name: "ParticipantContactInfo");

            migrationBuilder.DropTable(
                name: "UserAccountRoles");

            migrationBuilder.DropTable(
                name: "UserAddresses");

            migrationBuilder.DropTable(
                name: "UserAttributes");

            migrationBuilder.DropTable(
                name: "UserContactInfo");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "Criteria");

            migrationBuilder.DropTable(
                name: "Openings");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "AccountRoles");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Attributes");

            migrationBuilder.DropTable(
                name: "ContactInfo");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
