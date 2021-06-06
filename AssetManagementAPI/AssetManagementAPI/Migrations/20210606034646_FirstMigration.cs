using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetManagementAPI.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_M_Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_M_Items_TB_M_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TB_M_Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_M_Users_TB_M_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "TB_M_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_M_Users_TB_M_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "TB_M_Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_M_Accounts_TB_M_Users_Id",
                        column: x => x.Id,
                        principalTable: "TB_M_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_T_RequestItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_T_RequestItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_T_RequestItems_TB_M_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "TB_M_Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_T_RequestItems_TB_M_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TB_M_Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_T_RequestItems_TB_M_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "TB_M_Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_T_RoleAccounts",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_T_RoleAccounts", x => new { x.AccountId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_TB_T_RoleAccounts_TB_M_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "TB_M_Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_T_RoleAccounts_TB_M_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "TB_M_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_T_ReturnItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestItemId = table.Column<int>(type: "int", nullable: false),
                    Penalty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_T_ReturnItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_T_ReturnItems_TB_T_RequestItems_RequestItemId",
                        column: x => x.RequestItemId,
                        principalTable: "TB_T_RequestItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Items_CategoryId",
                table: "TB_M_Items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Users_DepartmentId",
                table: "TB_M_Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Users_GenderId",
                table: "TB_M_Users",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_RequestItems_AccountId",
                table: "TB_T_RequestItems",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_RequestItems_ItemId",
                table: "TB_T_RequestItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_RequestItems_StatusId",
                table: "TB_T_RequestItems",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_ReturnItems_RequestItemId",
                table: "TB_T_ReturnItems",
                column: "RequestItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_RoleAccounts_RoleId",
                table: "TB_T_RoleAccounts",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_T_ReturnItems");

            migrationBuilder.DropTable(
                name: "TB_T_RoleAccounts");

            migrationBuilder.DropTable(
                name: "TB_T_RequestItems");

            migrationBuilder.DropTable(
                name: "TB_M_Roles");

            migrationBuilder.DropTable(
                name: "TB_M_Accounts");

            migrationBuilder.DropTable(
                name: "TB_M_Items");

            migrationBuilder.DropTable(
                name: "TB_M_Statuses");

            migrationBuilder.DropTable(
                name: "TB_M_Users");

            migrationBuilder.DropTable(
                name: "TB_M_Categories");

            migrationBuilder.DropTable(
                name: "TB_M_Departments");

            migrationBuilder.DropTable(
                name: "TB_M_Genders");
        }
    }
}
