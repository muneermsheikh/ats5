using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserType = table.Column<string>(type: "TEXT", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationNo = table.Column<int>(type: "INTEGER", nullable: false),
                    Gender = table.Column<string>(type: "TEXT", maxLength: 1, nullable: false),
                    FirstName = table.Column<string>(type: "VARCHAR", unicode: false, maxLength: 75, nullable: false),
                    SecondName = table.Column<string>(type: "VARCHAR", unicode: false, maxLength: 75, nullable: true),
                    FamilyName = table.Column<string>(type: "VARCHAR", unicode: false, maxLength: 75, nullable: true),
                    KnownAs = table.Column<string>(type: "TEXT", nullable: false),
                    DOB = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlaceOfBirth = table.Column<string>(type: "TEXT", nullable: true),
                    AadharNo = table.Column<string>(type: "TEXT", nullable: true),
                    PpNo = table.Column<string>(type: "TEXT", nullable: true),
                    Nationality = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastActive = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Introduction = table.Column<string>(type: "TEXT", nullable: true),
                    Interests = table.Column<string>(type: "TEXT", nullable: true),
                    AppUserIdNotEnforced = table.Column<string>(type: "TEXT", nullable: true),
                    CandidateStatus = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistHRDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SrNo = table.Column<int>(type: "INTEGER", nullable: false),
                    Parameter = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistHRDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerType = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    CustomerName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    KnownAs = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Add = table.Column<string>(type: "TEXT", nullable: true),
                    Add2 = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    Pin = table.Column<string>(type: "TEXT", nullable: true),
                    District = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeployStages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Sequence = table.Column<int>(type: "INTEGER", nullable: false),
                    NextDeployStageSequence = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeployStages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    AppUserId = table.Column<string>(type: "TEXT", nullable: true),
                    Person_UserType = table.Column<string>(type: "TEXT", nullable: true),
                    Person_Gender = table.Column<string>(type: "TEXT", nullable: true),
                    Person_FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    Person_SecondName = table.Column<string>(type: "TEXT", nullable: true),
                    Person_FamilyName = table.Column<string>(type: "TEXT", nullable: true),
                    Person_KnownAs = table.Column<string>(type: "TEXT", nullable: true),
                    Person_DOB = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Person_PlaceOfBirth = table.Column<string>(type: "TEXT", nullable: true),
                    Person_AadharNo = table.Column<string>(type: "TEXT", nullable: true),
                    Person_PpNo = table.Column<string>(type: "TEXT", nullable: true),
                    Person_Nationality = table.Column<string>(type: "TEXT", nullable: true),
                    Person_Email = table.Column<string>(type: "TEXT", nullable: true),
                    Person_UserName = table.Column<string>(type: "TEXT", nullable: true),
                    DOJ = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Department = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastWorkingDay = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Remarks = table.Column<string>(type: "TEXT", nullable: true),
                    AadharNo = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastActive = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Add = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Industries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    IsMain = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Qualifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewItemDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SrNo = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewDescription = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewItemDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewItemStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemStatus = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewItemStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SkillName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaskType = table.Column<string>(type: "TEXT", nullable: true),
                    TaskDate = table.Column<DateTime>(type: "TEXT", maxLength: 250, nullable: false),
                    TaskOwnerId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedToId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: true),
                    OrderItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    TaskDescription = table.Column<string>(type: "TEXT", nullable: true),
                    CompleteBy = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TaskStatus = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressType = table.Column<string>(type: "TEXT", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", maxLength: 1, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    SecondName = table.Column<string>(type: "TEXT", nullable: true),
                    FamilyName = table.Column<string>(type: "TEXT", nullable: true),
                    DOB = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Add = table.Column<string>(type: "TEXT", nullable: true),
                    StreetAdd = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    District = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    Pin = table.Column<string>(type: "TEXT", nullable: true),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    AppUserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SenderId = table.Column<int>(type: "INTEGER", nullable: false),
                    SenderUsername = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SenderId1 = table.Column<string>(type: "TEXT", nullable: true),
                    RecipientId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecipientUsername = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RecipientId1 = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    DateRead = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MessageSent = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SenderDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RecipientDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AppUser_RecipientId1",
                        column: x => x.RecipientId1,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_AppUser_SenderId1",
                        column: x => x.SenderId1,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntityAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressType = table.Column<string>(type: "TEXT", nullable: true),
                    Add = table.Column<string>(type: "TEXT", unicode: false, maxLength: 250, nullable: false),
                    StreetAdd = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    Pin = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    District = table.Column<string>(type: "TEXT", nullable: true),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    IsMain = table.Column<bool>(type: "INTEGER", nullable: false),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityAddress_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false),
                    AppUserId = table.Column<string>(type: "TEXT", nullable: true),
                    AttachmentType = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    AttachmentSizeInKB = table.Column<int>(type: "INTEGER", nullable: false),
                    AttachmentUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAttachments_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false),
                    SrNo = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Employer = table.Column<string>(type: "TEXT", nullable: true),
                    Position = table.Column<string>(type: "TEXT", nullable: true),
                    SalaryCurrency = table.Column<string>(type: "TEXT", nullable: true),
                    MonthlySalaryDrawn = table.Column<int>(type: "INTEGER", nullable: true),
                    WorkedFrom = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WorkedUpto = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExps_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPassports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false),
                    PassportNo = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Nationality = table.Column<string>(type: "TEXT", nullable: true),
                    IssuedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Validity = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsValid = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPassports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPassports_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    IndustryId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsMain = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfessions_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserQualifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false),
                    QualificationId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsMain = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQualifications_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerIndustries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    IndustryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerIndustries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerIndustries_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOfficials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Gender = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    OfficialName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Designation = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNo = table.Column<string>(type: "TEXT", nullable: true),
                    Mobile = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    IsValid = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOfficials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerOfficials_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderNo = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderAddress_CompanyName = table.Column<string>(type: "TEXT", nullable: true),
                    OrderAddress_Add = table.Column<string>(type: "TEXT", nullable: true),
                    OrderAddress_StreetAdd = table.Column<string>(type: "TEXT", nullable: true),
                    OrderAddress_Location = table.Column<string>(type: "TEXT", nullable: true),
                    OrderAddress_City = table.Column<string>(type: "TEXT", nullable: true),
                    OrderAddress_District = table.Column<string>(type: "TEXT", nullable: true),
                    OrderAddress_State = table.Column<string>(type: "TEXT", nullable: true),
                    OrderAddress_Pin = table.Column<string>(type: "TEXT", nullable: true),
                    OrderAddress_Country = table.Column<string>(type: "TEXT", nullable: true),
                    OrderAddress_OrderId1 = table.Column<int>(type: "INTEGER", nullable: true),
                    OrderDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    BuyerEmail = table.Column<string>(type: "TEXT", nullable: true),
                    OrderRef = table.Column<string>(type: "TEXT", nullable: true),
                    SalesmanId = table.Column<int>(type: "INTEGER", nullable: true),
                    SalesmanName = table.Column<string>(type: "TEXT", nullable: true),
                    CompleteBy = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    CityOfWorking = table.Column<string>(type: "TEXT", nullable: false),
                    EstimatedRevenue = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Orders_OrderAddress_OrderId1",
                        column: x => x.OrderAddress_OrderId1,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeHRSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    IndustryId = table.Column<int>(type: "INTEGER", nullable: false),
                    SkillLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeHRSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeHRSkills_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeOtherSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    SkillDataId = table.Column<int>(type: "INTEGER", nullable: false),
                    SkillLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    IsMain = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeOtherSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeOtherSkills_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeQualifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    QualificationId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsMain = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeQualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeQualifications_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserPhones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false),
                    PhoneNo = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    MobileNo = table.Column<string>(type: "TEXT", nullable: true),
                    IsMain = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsValid = table.Column<bool>(type: "INTEGER", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPhones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPhones_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPhones_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    ConnectionId = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    GroupName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.ConnectionId);
                    table.ForeignKey(
                        name: "FK_Connections_Groups_GroupName",
                        column: x => x.GroupName,
                        principalTable: "Groups",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderNo = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: false),
                    ReviewedBy = table.Column<int>(type: "INTEGER", nullable: true),
                    ReviewedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ReviewStatusId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractReviews_ReviewStatuses_ReviewStatusId",
                        column: x => x.ReviewStatusId,
                        principalTable: "ReviewStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TaskStatus = table.Column<string>(type: "TEXT", nullable: false),
                    TaskItemDescription = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    NextFollowupOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NextFollowupById = table.Column<int>(type: "INTEGER", nullable: true),
                    TaskId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskItems_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskItems_Tasks_TaskId1",
                        column: x => x.TaskId1,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    SrNo = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: true),
                    IndustryId = table.Column<int>(type: "INTEGER", nullable: false),
                    IndustryName = table.Column<string>(type: "TEXT", nullable: true),
                    SourceFrom = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    MinCVs = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxCVs = table.Column<int>(type: "INTEGER", nullable: false),
                    Ecnr = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequireAssess = table.Column<bool>(type: "INTEGER", nullable: false),
                    CompleteBefore = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HrExecId = table.Column<int>(type: "INTEGER", nullable: true),
                    HRExecName = table.Column<string>(type: "TEXT", nullable: true),
                    HrSupId = table.Column<int>(type: "INTEGER", nullable: true),
                    HrSupName = table.Column<string>(type: "TEXT", nullable: true),
                    HrmId = table.Column<int>(type: "INTEGER", nullable: true),
                    HrmName = table.Column<string>(type: "TEXT", nullable: true),
                    AssignedId = table.Column<int>(type: "INTEGER", nullable: true),
                    AssignedToName = table.Column<string>(type: "TEXT", nullable: true),
                    Charges = table.Column<int>(type: "INTEGER", nullable: false),
                    FeeFromClientINR = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ReviewItemStatusId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Employees_AssignedId",
                        column: x => x.AssignedId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistHRs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CheckedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistHRs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistHRs_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChecklistHRs_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractReviewItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Ecnr = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequireAssess = table.Column<bool>(type: "INTEGER", nullable: false),
                    SourceFrom = table.Column<string>(type: "TEXT", nullable: true),
                    ReviewItemStatus = table.Column<string>(type: "TEXT", nullable: false),
                    ContractReviewId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractReviewItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractReviewItems_ContractReviews_ContractReviewId",
                        column: x => x.ContractReviewId,
                        principalTable: "ContractReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractReviewItems_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CVRefs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReferredOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeployStageId = table.Column<int>(type: "INTEGER", nullable: true),
                    RefStatusId = table.Column<int>(type: "INTEGER", nullable: false),
                    Charges = table.Column<int>(type: "INTEGER", nullable: false),
                    PaymentIntentId = table.Column<string>(type: "TEXT", nullable: true),
                    RefStatus = table.Column<string>(type: "TEXT", nullable: false),
                    DeployStageDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    OrderItemId1 = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderItemId2 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVRefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVRefs_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CVRefs_DeployStages_DeployStageId",
                        column: x => x.DeployStageId,
                        principalTable: "DeployStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CVRefs_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CVRefs_OrderItems_OrderItemId1",
                        column: x => x.OrderItemId1,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobDescriptions",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    JobDescInBrief = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    QualificationDesired = table.Column<string>(type: "TEXT", nullable: true),
                    ExpDesiredMin = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpDesiredMax = table.Column<int>(type: "INTEGER", nullable: false),
                    MinAge = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxAge = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDescriptions", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_JobDescriptions_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Remunerations",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    SalaryCurrency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    SalaryMin = table.Column<int>(type: "INTEGER", nullable: false),
                    SalaryMax = table.Column<int>(type: "INTEGER", nullable: false),
                    ContractPeriodInMonths = table.Column<int>(type: "INTEGER", nullable: false),
                    HousingProvidedFree = table.Column<bool>(type: "INTEGER", nullable: false),
                    HousingAllowance = table.Column<int>(type: "INTEGER", nullable: false),
                    FoodProvidedFree = table.Column<bool>(type: "INTEGER", nullable: false),
                    FoodAllowance = table.Column<int>(type: "INTEGER", nullable: false),
                    TransportProvidedFree = table.Column<bool>(type: "INTEGER", nullable: false),
                    TransportAllowance = table.Column<int>(type: "INTEGER", nullable: false),
                    OtherAllowance = table.Column<int>(type: "INTEGER", nullable: false),
                    LeavePerYearInDays = table.Column<int>(type: "INTEGER", nullable: false),
                    LeaveAirfareEntitlementAfterMonths = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remunerations", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_Remunerations_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckListItemHRs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChecklistHRId = table.Column<int>(type: "INTEGER", nullable: false),
                    SrNo = table.Column<int>(type: "INTEGER", nullable: false),
                    Parameter = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Accepts = table.Column<bool>(type: "INTEGER", nullable: false),
                    Response = table.Column<string>(type: "TEXT", nullable: true),
                    Exceptions = table.Column<string>(type: "TEXT", nullable: true),
                    MandatoryTrue = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListItemHRs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckListItemHRs_ChecklistHRs_ChecklistHRId",
                        column: x => x.ChecklistHRId,
                        principalTable: "ChecklistHRs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CVDeploys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CVRefId = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeployStageId = table.Column<int>(type: "INTEGER", nullable: false),
                    NextDeployStageId = table.Column<int>(type: "INTEGER", nullable: false),
                    NextDeployStageEstimatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CVRefId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVDeploys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVDeploys_CVRefs_CVRefId",
                        column: x => x.CVRefId,
                        principalTable: "CVRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CVDeploys_CVRefs_CVRefId1",
                        column: x => x.CVRefId1,
                        principalTable: "CVRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_AppUserId",
                table: "Address",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_ApplicationNo",
                table: "Candidates",
                column: "ApplicationNo",
                unique: true,
                filter: "[ApplicationNo] > 0");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistHRDatas_Parameter",
                table: "ChecklistHRDatas",
                column: "Parameter",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistHRDatas_SrNo",
                table: "ChecklistHRDatas",
                column: "SrNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistHRs_CandidateId_OrderItemId",
                table: "ChecklistHRs",
                columns: new[] { "CandidateId", "OrderItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistHRs_OrderItemId",
                table: "ChecklistHRs",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListItemHRs_ChecklistHRId",
                table: "CheckListItemHRs",
                column: "ChecklistHRId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_GroupName",
                table: "Connections",
                column: "GroupName");

            migrationBuilder.CreateIndex(
                name: "IX_ContractReviewItems_ContractReviewId",
                table: "ContractReviewItems",
                column: "ContractReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractReviewItems_OrderItemId",
                table: "ContractReviewItems",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractReviews_OrderId",
                table: "ContractReviews",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractReviews_OrderNo",
                table: "ContractReviews",
                column: "OrderNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractReviews_ReviewStatusId",
                table: "ContractReviews",
                column: "ReviewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerIndustries_CustomerId_IndustryId",
                table: "CustomerIndustries",
                columns: new[] { "CustomerId", "IndustryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOfficials_CustomerId",
                table: "CustomerOfficials",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerName_City",
                table: "Customers",
                columns: new[] { "CustomerName", "City" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CVDeploys_CVRefId",
                table: "CVDeploys",
                column: "CVRefId");

            migrationBuilder.CreateIndex(
                name: "IX_CVDeploys_CVRefId1",
                table: "CVDeploys",
                column: "CVRefId1");

            migrationBuilder.CreateIndex(
                name: "IX_CVRefs_CandidateId_OrderItemId",
                table: "CVRefs",
                columns: new[] { "CandidateId", "OrderItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CVRefs_DeployStageId",
                table: "CVRefs",
                column: "DeployStageId");

            migrationBuilder.CreateIndex(
                name: "IX_CVRefs_OrderItemId",
                table: "CVRefs",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CVRefs_OrderItemId1",
                table: "CVRefs",
                column: "OrderItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_DeployStages_Sequence",
                table: "DeployStages",
                column: "Sequence",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeployStages_Status",
                table: "DeployStages",
                column: "Status",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHRSkills_EmployeeId_CategoryId_IndustryId",
                table: "EmployeeHRSkills",
                columns: new[] { "EmployeeId", "CategoryId", "IndustryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOtherSkills_EmployeeId_SkillDataId",
                table: "EmployeeOtherSkills",
                columns: new[] { "EmployeeId", "SkillDataId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeQualifications_EmployeeId_QualificationId",
                table: "EmployeeQualifications",
                columns: new[] { "EmployeeId", "QualificationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntityAddress_CandidateId",
                table: "EntityAddress",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Industries_Name",
                table: "Industries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientId1",
                table: "Messages",
                column: "RecipientId1");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId1",
                table: "Messages",
                column: "SenderId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_AssignedId",
                table: "OrderItems",
                column: "AssignedId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CategoryId",
                table: "OrderItems",
                column: "CategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderAddress_OrderId1",
                table: "Orders",
                column: "OrderAddress_OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Qualifications_Name",
                table: "Qualifications",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewItemDatas_SrNo",
                table: "ReviewItemDatas",
                column: "SrNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewItemStatuses_ItemStatus",
                table: "ReviewItemStatuses",
                column: "ItemStatus",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewStatuses_Status",
                table: "ReviewStatuses",
                column: "Status",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillDatas_SkillName",
                table: "SkillDatas",
                column: "SkillName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_TaskId",
                table: "TaskItems",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_TaskId1",
                table: "TaskItems",
                column: "TaskId1");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_TransactionDate",
                table: "TaskItems",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_UserId",
                table: "TaskItems",
                column: "UserId",
                filter: "[UserId]>0");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedToId",
                table: "Tasks",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskOwnerId",
                table: "Tasks",
                column: "TaskOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskType",
                table: "Tasks",
                column: "TaskType");

            migrationBuilder.CreateIndex(
                name: "IX_UserAttachments_AttachmentUrl",
                table: "UserAttachments",
                column: "AttachmentUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAttachments_CandidateId",
                table: "UserAttachments",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExps_CandidateId",
                table: "UserExps",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPassports_CandidateId",
                table: "UserPassports",
                column: "CandidateId",
                filter: "[IsValid]=1");

            migrationBuilder.CreateIndex(
                name: "IX_UserPassports_PassportNo",
                table: "UserPassports",
                column: "PassportNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPhones_CandidateId",
                table: "UserPhones",
                column: "CandidateId",
                filter: "[IsValid]=1");

            migrationBuilder.CreateIndex(
                name: "IX_UserPhones_EmployeeId",
                table: "UserPhones",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPhones_PhoneNo",
                table: "UserPhones",
                column: "PhoneNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfessions_CandidateId",
                table: "UserProfessions",
                column: "CandidateId",
                filter: "[IsMain]=1");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfessions_CandidateId_CategoryId_IndustryId",
                table: "UserProfessions",
                columns: new[] { "CandidateId", "CategoryId", "IndustryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserQualifications_CandidateId",
                table: "UserQualifications",
                column: "CandidateId",
                filter: "[IsMain]=1");

            migrationBuilder.CreateIndex(
                name: "IX_UserQualifications_CandidateId_QualificationId",
                table: "UserQualifications",
                columns: new[] { "CandidateId", "QualificationId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "ChecklistHRDatas");

            migrationBuilder.DropTable(
                name: "CheckListItemHRs");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "ContractReviewItems");

            migrationBuilder.DropTable(
                name: "CustomerIndustries");

            migrationBuilder.DropTable(
                name: "CustomerOfficials");

            migrationBuilder.DropTable(
                name: "CVDeploys");

            migrationBuilder.DropTable(
                name: "EmployeeHRSkills");

            migrationBuilder.DropTable(
                name: "EmployeeOtherSkills");

            migrationBuilder.DropTable(
                name: "EmployeeQualifications");

            migrationBuilder.DropTable(
                name: "EntityAddress");

            migrationBuilder.DropTable(
                name: "Industries");

            migrationBuilder.DropTable(
                name: "JobDescriptions");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Qualifications");

            migrationBuilder.DropTable(
                name: "Remunerations");

            migrationBuilder.DropTable(
                name: "ReviewItemDatas");

            migrationBuilder.DropTable(
                name: "ReviewItemStatuses");

            migrationBuilder.DropTable(
                name: "SkillDatas");

            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.DropTable(
                name: "UserAttachments");

            migrationBuilder.DropTable(
                name: "UserExps");

            migrationBuilder.DropTable(
                name: "UserPassports");

            migrationBuilder.DropTable(
                name: "UserPhones");

            migrationBuilder.DropTable(
                name: "UserProfessions");

            migrationBuilder.DropTable(
                name: "UserQualifications");

            migrationBuilder.DropTable(
                name: "ChecklistHRs");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "ContractReviews");

            migrationBuilder.DropTable(
                name: "CVRefs");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "ReviewStatuses");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "DeployStages");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
