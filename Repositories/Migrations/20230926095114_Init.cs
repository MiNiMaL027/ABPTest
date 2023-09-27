using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Experements",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experements", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Experements",
                columns: new[] { "Name", "Value" },
                values: new object[] { "button_color", "#FF0000" }); // Додаю дані до бази даних через міграції

            migrationBuilder.InsertData(
                table: "Experements",
                columns: new[] { "Name", "Value" },
                values: new object[] { "button_color", "#00FF00" });

            migrationBuilder.InsertData(
                table: "Experements",
                columns: new[] { "Name", "Value" },
                values: new object[] { "button_color", "#0000FF" });

            migrationBuilder.InsertData(
              table: "Experements",
              columns: new[] { "Name", "Value" },
              values: new object[] { "price", "10" });

            migrationBuilder.InsertData(
             table: "Experements",
             columns: new[] { "Name", "Value" },
             values: new object[] { "price", "20" });

            migrationBuilder.InsertData(
             table: "Experements",
             columns: new[] { "Name", "Value" },
             values: new object[] { "price", "50" });

            migrationBuilder.InsertData(
             table: "Experements",
             columns: new[] { "Name", "Value" },
             values: new object[] { "price", "5" });

            migrationBuilder.Sql(@"USE [ABPTest]
                GO

                /****** Object:  StoredProcedure [dbo].[GetChosenExperementIdByExperementName]    Script Date: 9/27/2023 1:07:33 PM ******/
                SET ANSI_NULLS ON
                GO

                SET QUOTED_IDENTIFIER ON
                GO

                CREATE PROCEDURE [dbo].[GetChosenExperementIdByExperementName]
                    @ExperementName NVARCHAR(MAX)
                AS
                BEGIN
                    -- Визначення бажаних відношень
                    DECLARE @DesiredRatios TABLE (ExperementId INT, Percentage DECIMAL(18, 2))

                    INSERT INTO @DesiredRatios (ExperementId, Percentage)
                    VALUES
                        (4, 75),
                        (5, 10),
                        (6, 5),
                        (7, 10);

                    -- Розрахунок поточного відношення для заданого експерименту
                    WITH CurrentRatios AS (
                        SELECT
                            E.id AS ExperementId,
                            COUNT(TE.ExperementId) * 1.0 / NULLIF(Total.TotalCount, 0) * 100 AS Percentage
                        FROM Experements E
                        INNER JOIN TokenExperements TE ON E.id = TE.ExperementId
                        CROSS JOIN (
                            SELECT COUNT(*) AS TotalCount FROM TokenExperements JOIN Experements ON TokenExperements.ExperementId = Experements.id WHERE Experements.Name = @ExperementName
                        ) AS Total
                        WHERE E.Name = @ExperementName
                        GROUP BY E.id, Total.TotalCount
                    )

                    -- Вибір id з найменшим процентом, який менший за потрібний
                    SELECT TOP 1
                        CR.ExperementId AS ChosenExperementId
                    FROM CurrentRatios CR
                    WHERE EXISTS (
                        SELECT 1
                        FROM @DesiredRatios DR
                        WHERE CR.ExperementId = DR.ExperementId
                        AND CR.Percentage < DR.Percentage
                    )
                    ORDER BY CR.Percentage ASC;
                END;
                GO
            "); // Тут я додаю Stored Prosedure 

            migrationBuilder.Sql(@"USE [ABPTest]
                GO

                /****** Object:  StoredProcedure [dbo].[GetExperementToAddToTokenExperement]    Script Date: 9/27/2023 1:09:44 PM ******/
                SET ANSI_NULLS ON
                GO

                SET QUOTED_IDENTIFIER ON
                GO

                CREATE PROCEDURE [dbo].[GetExperementToAddToTokenExperement]
                    @ExperementName NVARCHAR(MAX)
                AS
                BEGIN
                    -- Повернення одного id експерименту з таблиці Experements, де Name відповідає заданому значенню,
                    -- і це id ще не існує в таблиці TokenExperement

                    DECLARE @ExperementId INT;

                    SELECT TOP 1 @ExperementId = E.id
                    FROM Experements E
                    WHERE E.Name = @ExperementName
                    AND NOT EXISTS (
                        SELECT 1
                        FROM TokenExperements TE
                        WHERE TE.ExperementId = E.id
                    );

                    SELECT @ExperementId AS ExperementId;
                END;
                GO"
            );

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TokenExperements",
                columns: table => new
                {
                    TokenId = table.Column<int>(type: "int", nullable: false),
                    ExperementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenExperements", x => new { x.TokenId, x.ExperementId });
                    table.ForeignKey(
                        name: "FK_TokenExperements_Experements_ExperementId",
                        column: x => x.ExperementId,
                        principalTable: "Experements",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TokenExperements_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalTable: "Tokens",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokenExperements_ExperementId",
                table: "TokenExperements",
                column: "ExperementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenExperements");

            migrationBuilder.DropTable(
                name: "Experements");

            migrationBuilder.DropTable(
                name: "Tokens");
        }
    }
}
