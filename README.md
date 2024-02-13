# RanksPoints Module - GeoIP
# EN
**RanksPoints Module - GeoIP** - is an addon designed for integration with the Ranks Points plugin, and it also ensures compatibility with other plugins using the database from Levels Ranks. This module is responsible for collecting data on players' locations, including their IP addresses, countries, regions, and cities. The gathered information is exclusively accessible through the Web Interface.

# Installation
1. Install [Metamod:Source](https://www.sourcemm.net/downloads.php/?branch=master) and [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp)
2. Download the RanksPoints Module - GeoIP.
3. Unpack the archive and upload it to the game server.
4. Start the server to create the necessary configuration files.
5. Connect the plugin to the database by entering the required details in the dbconfig.json file. Ensure the accuracy of the entered data.

# Database Connection Configuration (dbconfig.json)
IMPORTANT: The suffix _geoip is added automatically; you only need to specify the name of your database.
```
{
  "DbHost": "YourHost",
  "DbUser": "YourUser",
  "DbPassword": "YourPassword",
  "DbName": "YourDatabase",
  "DbPort": "3310",
  "TableName": "lvl_base"
}
```

# RU
**RanksPoints Module - GeoIP** - это дополнение, предназначенное для интеграции с плагином Ranks Points, а также обеспечивает совместимость с другими плагинами, использующими базу данных от Levels Ranks. Этот модуль занимается сбором данных о местоположении игроков, включая их IP-адреса, страны, регионы и города. Собранная информация доступна исключительно через Веб-Интерфейс.

# Установка
1. Установите [Metamod:Source](https://www.sourcemm.net/downloads.php/?branch=master) и [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp)
2. Скачайте RanksPoints Module - GeoIP.
3. Распакуйте архив и загрузите его на игровой сервер.
4. Запустите сервер, чтобы создать необходимые конфигурационные файлы.
5. Подключите плагин к базе данных, введя необходимые данные в файл dbconfig.json. Убедитесь в корректности введенных данных.

# Конфиг подключения базы данных (dbconfig.json)
ВАЖНО: Суффикс _geoip добавляется автоматически; необходимо указать только имя вашей базы данных.
```
{
  "DbHost": "YourHost",
  "DbUser": "YourUser",
  "DbPassword": "YourPassword",
  "DbName": "YourDatabase",
  "DbPort": "3310",
  "TableName": "lvl_base"
}
```
