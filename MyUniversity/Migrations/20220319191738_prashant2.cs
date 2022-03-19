using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyUniversity.Migrations
{
    public partial class prashant2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EnrollmentID",
                table: "Students",
                type: "int",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "Theses",
                columns: table => new
                {
                    ThesisId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThesisName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThesisDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectId = table.Column<short>(type: "smallint", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletionPercent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theses", x => x.ThesisId);
                    table.ForeignKey(
                        name: "FK_Theses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Theses_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubmissionDetails",
                columns: table => new
                {
                    SubmissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThesisId = table.Column<short>(type: "smallint", nullable: false),
                    varchar = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    SubmissionDueOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmissionOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmissionFile = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FileContentType = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    ReviewedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    remark = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    SubmissionStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionDetails", x => x.SubmissionId);
                    table.ForeignKey(
                        name: "FK_SubmissionDetails_Theses_ThesisId",
                        column: x => x.ThesisId,
                        principalTable: "Theses",
                        principalColumn: "ThesisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionDetails_ThesisId",
                table: "SubmissionDetails",
                column: "ThesisId");

            migrationBuilder.CreateIndex(
                name: "IX_Theses_StudentId",
                table: "Theses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Theses_SubjectId",
                table: "Theses",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubmissionDetails");

            migrationBuilder.DropTable(
                name: "Theses");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.AlterColumn<string>(
                name: "EnrollmentID",
                table: "Students",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 10);
        }
    }
}
