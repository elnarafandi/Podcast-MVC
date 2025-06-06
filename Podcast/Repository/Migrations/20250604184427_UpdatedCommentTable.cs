using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Podcasts_PodcastId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PodcastId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PodcastId1",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "PodcastId",
                table: "Comments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PodcastId",
                table: "Comments",
                column: "PodcastId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Podcasts_PodcastId",
                table: "Comments",
                column: "PodcastId",
                principalTable: "Podcasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Podcasts_PodcastId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PodcastId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "PodcastId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PodcastId1",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PodcastId1",
                table: "Comments",
                column: "PodcastId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Podcasts_PodcastId1",
                table: "Comments",
                column: "PodcastId1",
                principalTable: "Podcasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
