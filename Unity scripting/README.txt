IoTData is the script that downloads the IoT data from the ThingSpeak server and
writes it in a csv file.

CloudsBehavior is the script that takes weather data from the API of OpenWeather and
changes the state of the application sky from this data.

ScreenData is the script that updates the data showed to the user playing the application.
That data correspond to the reads of the sensors of the IoT device.

Sun is the script that update the Sun position and also the day and night of the
application.

SensorResponse is a class that has the same data structure than the response of a reading
petition to the ThingSpeak server.

wEATHERResponse is a class that has the same data structure than the response of a reading
petition to the weather data platform.
