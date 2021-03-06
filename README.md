# VORP-CORE
This is the VORP CORE for RedM made in C# compatible with LUA
[Discord VORP](https://discord.gg/23MPbQ6)

## Requirements
- [ghmattimysql](https://github.com/GHMatti/ghmattimysql/releases)

## How to install
* Download ghmattimysql
* Copy and paste ```ghmattimysql``` folder to ```resources/ghmattimysql```
* Delete file ```resources/ghmattimysql/config.json```
* Add ```set mysql_connection_string "mysql://mysqluser:password@localhost/vorp"``` to your ```server.cfg``` file
* Add ```ensure ghmattimysql``` to your ```server.cfg``` file

* [Download the lastest version of VORP CORE](https://github.com/VORPCORE/VORP-Core/releases)

* Copy and paste ```vorp_core``` folder to ```resources/vorp_core```
* Add ```ensure vorp_core``` to your ```server.cfg``` file
* To change the language go to ```resources/vorp_core/Config``` and change the default language
* Example Server.cfg
```cfg
#MYSQL
set mysql_connection_string "mysql://user:password@ip/vorp"
ensure ghmattimysql
#Core
ensure vorp_core
```
````
The vorp_core folder has a vorp.sql file, you need to create a database 'vorp' and import the sql.
````
Now you are ready!

## Wiki
[Wiki VORP Core](http://docs.vorpcore.com:3000/home)
