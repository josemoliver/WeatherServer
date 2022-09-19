# WeatherServer
Console app which reads Ambient Weather Protocol for local weather station storage.


## Introduction
This console app will listen to the HTTP messages sent by the weather station and write them to a SQLlite data base. You will need to build and create the sqlite database (Table create command included). You may need to customize the CREATE TABLE command depending on your PWS model and the data which it sends. 
Refer to https://ambientweather.com/faqs/question/view/id/1857 for the available parameters.


## Awnet Advanced Settings

![AWNet Advanced Settings](https://github.com/josemoliver/WeatherServer/blob/main/AWNet_Advanced_Settings.png)
