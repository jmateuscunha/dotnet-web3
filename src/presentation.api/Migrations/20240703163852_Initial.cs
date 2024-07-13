using System;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace application.api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blockchain",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    network = table.Column<string>(type: "text", nullable: false),
                    chain_id = table.Column<int>(type: "int4", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blockchain", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wallet",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<long>(type: "bigint", nullable: false),
                    blockchain_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet", x => x.id);
                    table.ForeignKey(
                        name: "FK_wallet_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_wallet_blockchain_blockchain_id",
                        column: x => x.blockchain_id,
                        principalTable: "blockchain",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accounts_roles",
                columns: table => new
                {
                    AccountsId = table.Column<long>(type: "bigint", nullable: false),
                    RolesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts_roles", x => new { x.AccountsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_accounts_roles_account_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_accounts_roles_role_RolesId",
                        column: x => x.RolesId,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asset",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    wallet_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    balance = table.Column<BigInteger>(type: "numeric(1000,0)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    keystore = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset", x => x.id);
                    table.ForeignKey(
                        name: "FK_asset_wallet_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "wallet",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: false),
                    from_address = table.Column<string>(type: "text", nullable: false),
                    to_address = table.Column<string>(type: "text", nullable: false),
                    amount_deducted = table.Column<BigInteger>(type: "numeric(1000,0)", nullable: false),
                    amount_requested = table.Column<BigInteger>(type: "numeric(1000,0)", nullable: false),
                    fee_applied = table.Column<BigInteger>(type: "numeric(1000,0)", nullable: false),
                    nonce = table.Column<BigInteger>(type: "numeric(1000,0)", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "PENDING"),
                    block_number = table.Column<BigInteger>(type: "numeric(1000,0)", nullable: true),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_transaction_asset_asset_id",
                        column: x => x.asset_id,
                        principalTable: "asset",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "blockchain",
                columns: new[] { "id", "chain_id", "created_at", "name", "network", "updated_at" },
                values: new object[] { new Guid("5b38332c-9fa4-4215-bbec-87a8588e2edf"), 11155111, new DateTime(2024, 7, 3, 16, 38, 52, 585, DateTimeKind.Utc).AddTicks(8457), "Ethereum-Sepolia", "Testnet", new DateTime(2024, 7, 3, 13, 38, 52, 585, DateTimeKind.Local).AddTicks(8460) });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "id", "created_at", "name", "updated_at" },
                values: new object[] { -1, new DateTime(2024, 7, 3, 16, 38, 52, 585, DateTimeKind.Utc).AddTicks(8331), "ADMIN", new DateTime(2024, 7, 3, 16, 38, 52, 585, DateTimeKind.Utc).AddTicks(8333) });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_roles_RolesId",
                table: "accounts_roles",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_asset_wallet_id",
                table: "asset",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_asset_id",
                table: "transaction",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_wallet_account_id",
                table: "wallet",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_wallet_blockchain_id",
                table: "wallet",
                column: "blockchain_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts_roles");

            migrationBuilder.DropTable(
                name: "transaction");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "asset");

            migrationBuilder.DropTable(
                name: "wallet");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "blockchain");
        }
    }
}
