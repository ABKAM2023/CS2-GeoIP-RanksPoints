using System;
using System.IO;
using System.Threading.Tasks;
using MySqlConnector;
using System.Net;
using CounterStrikeSharp.API;
using Newtonsoft.Json;
using MaxMind.Db; 
using MaxMind.GeoIP2;
using System.Collections.Generic; 
using Dapper;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;

namespace GeoIpPlugin;

[MinimumApiVersion(80)]
public class GeoIpPlugin : BasePlugin
{
    public override string ModuleName => "[RanksPoints] GeoIP";
    public override string ModuleVersion => "1.0";
    public override string ModuleAuthor => "ABKAM";
    public override string ModuleDescription => "A plugin for GeoIP information.";
    private DatabaseConfig? dbConfig;
    private const string DbConfigFileName = "dbconfig.json";
    private const string CreateTableQuery = @"
        CREATE TABLE IF NOT EXISTS `{0}_geoip` (
            `steam` varchar(32) NOT NULL default '' PRIMARY KEY,
            `clientip` varchar(16) NOT NULL default '',
            `country` varchar(48) NOT NULL default '',
            `region` varchar(48) NOT NULL default '',
            `city` varchar(48) NOT NULL default '',
            `country_code` varchar(4) NOT NULL default ''
        ) CHARSET=utf8 COLLATE utf8_general_ci;";

    public override void Load(bool hotReload)
    {
        base.Load(hotReload);
        EnsureDbConfigExists();
        RegisterListener<Listeners.OnClientConnected>(OnClientConnected);
        dbConfig = JsonConvert.DeserializeObject<DatabaseConfig>(File.ReadAllText(Path.Combine(ModuleDirectory, DbConfigFileName)));
        CreateDbTableIfNotExists();
    }
    private void OnClientConnected(int playerSlot)
    {
        var player = Utilities.GetPlayerFromSlot(playerSlot);
        if (player != null && !player.IsBot)
        {
            var steamId64 = player.SteamID.ToString();
            var steamId = ConvertSteamID64ToSteamID(steamId64);
            var playerIp = player.IpAddress?.Split(':')[0]; 

            if (!string.IsNullOrEmpty(playerIp))
            {
                LogPlayerConnectionAsync(steamId, playerIp).ConfigureAwait(false);
            }
        }
    }
    private async Task LogPlayerConnectionAsync(string steamId, string playerIp)
    {
        try
        {
            using var reader = new DatabaseReader(Path.Combine(ModuleDirectory, "GeoLite2-City.mmdb"));
            var ipAddress = IPAddress.Parse(playerIp);
            var cityResponse = reader.City(ipAddress); 
            var countryName = cityResponse.Country.Names["en"];
            var regionName = cityResponse.MostSpecificSubdivision.Names["en"];
            var cityName = cityResponse.City.Names["en"];
            var countryCode = cityResponse.Country.IsoCode;

            using (var connection = new MySqlConnection(dbConfig.ConnectionString))
            {
                await connection.OpenAsync();
                var query = $@"
                INSERT INTO `{dbConfig.TableName}_geoip` (steam, clientip, country, region, city, country_code)
                VALUES (@SteamID, @ClientIP, @Country, @Region, @City, @CountryCode)
                ON DUPLICATE KEY UPDATE 
                    clientip = @ClientIP, 
                    country = @Country, 
                    region = @Region, 
                    city = @City, 
                    country_code = @CountryCode;";

                await connection.ExecuteAsync(query, new 
                {
                    SteamID = steamId, 
                    ClientIP = playerIp, 
                    Country = countryName, 
                    Region = regionName, 
                    City = cityName, 
                    CountryCode = countryCode
                });

                Console.WriteLine($"[GeoIP] Player {steamId} with IP {playerIp} and location {countryName}, {regionName}, {cityName} logged.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GeoIP] Error logging player connection for {steamId} with IP {playerIp}: {ex.Message}");
        }
    }
    private void EnsureDbConfigExists()
    {
        string configFilePath = Path.Combine(ModuleDirectory, DbConfigFileName);
        if (!File.Exists(configFilePath))
        {
            var defaultConfig = new DatabaseConfig
            {
                DbHost = "localhost",
                DbUser = "root",
                DbPassword = "password",
                DbName = "your_db_name",
                DbPort = "3306",
                TableName = "lvl_base_geoip"
            };

            string jsonConfig = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented);
            File.WriteAllText(configFilePath, jsonConfig);
            Console.WriteLine("Database configuration file created.");
        }
    }
    private void CreateDbTableIfNotExists()
    {
        using var connection = new MySqlConnection(dbConfig.ConnectionString);
        connection.Open();
        
        var tableName = dbConfig.TableName; 
        var query = string.Format(CreateTableQuery, tableName);
    
        using var command = new MySqlCommand(query, connection);
        command.ExecuteNonQuery();
    }
    public class DatabaseConfig
    {
        public string? DbHost { get; set; }
        public string? DbUser { get; set; }
        public string? DbPassword { get; set; }
        public string? DbName { get; set; }
        public string? DbPort { get; set; }
        public string? TableName { get; set; } 

        [Newtonsoft.Json.JsonIgnore]
        public string ConnectionString => $"Server={DbHost};Port={DbPort};User ID={DbUser};Password={DbPassword};Database={DbName};";
    }
    public class GeoLite2Data
    {
        [Constructor]
        public GeoLite2Data() {}

        public Country Country { get; set; }
        public List<Subdivision> Subdivisions { get; set; }
        public City City { get; set; }
    }

    public class Country
    {
        public Dictionary<string, string> Names { get; set; }
        public string IsoCode { get; set; }
    }

    public class Subdivision
    {
        public Dictionary<string, string> Names { get; set; }
    }

    public class City
    {
        public Dictionary<string, string> Names { get; set; }
    }   
    public static string ConvertSteamID64ToSteamID(string steamId64)
    {
        if (ulong.TryParse(steamId64, out var communityId) && communityId > 76561197960265728)
        {
            var authServer = (communityId - 76561197960265728) % 2;
            var authId = (communityId - 76561197960265728 - authServer) / 2;
            return $"STEAM_1:{authServer}:{authId}";
        }
        return null; 
    }    
}
