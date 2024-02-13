# RanksPoints Module - GeoIP
**RanksPoints Module - GeoIP** - это дополнение, предназначенное для интеграции с плагином Ranks Points, а также обеспечивает совместимость с другими плагинами, использующими базу данных от Levels Ranks. Этот модуль занимается сбором данных о местоположении игроков, включая их IP-адреса, страны, регионы и города. Собранная информация доступна исключительно через Веб-Интерфейс.

# Установка
1. Установите [Metamod:Source](https://www.sourcemm.net/downloads.php/?branch=master) и [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp)
2. Скачайте RanksPoints Module - GeoIP.
3. Распакуйте архив и загрузите его на игровой сервер.
4. Запустите сервер, чтобы создать необходимые конфигурационные файлы.
5. Подключите плагин к базе данных, введя необходимые данные в файл dbconfig.json. Убедитесь в корректности введенных данных.

# Конфиг подключения базы данных (dbconfig.json)
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
