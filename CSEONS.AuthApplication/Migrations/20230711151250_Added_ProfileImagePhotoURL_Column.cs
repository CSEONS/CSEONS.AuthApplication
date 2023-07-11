using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSEONS.AuthApplication.Migrations
{
    /// <inheritdoc />
    public partial class Added_ProfileImagePhotoURL_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePhotoURL",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b12858e6-15b5-462a-aab8-bcdc557138d6",
                columns: new[] { "ConcurrencyStamp", "ImagePhotoURL", "PasswordHash" },
                values: new object[] { "640ad606-b9a2-4bf9-84ce-37104865bec5", null, "AQAAAAIAAYagAAAAEM6ijA+cahw8sInXa9B+QXRmrw6Qv45sspusB+0bdgFCGYiT84ERgYJ+MK4OKGmOyQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePhotoURL",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b12858e6-15b5-462a-aab8-bcdc557138d6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2c12255a-4f47-47d8-89b1-d6db07b34bc3", "AQAAAAIAAYagAAAAEItoNQ7A2WDxrIlj0Pb43mZosom9gmrbivdKJG3/DafRsvRNB5S4LMoSE4UaffDgBQ==" });
        }
    }
}
