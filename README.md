# MathProject-Capstone-
For my Math capstone class I focused on creating a dataset of different flight routes(Flights need 1-6) and their prices (Saved Flights all csv).
I want to study different mathematical models and see how different sceneraios affect the prices of flights.
Some of these scenarios which is information im pulling off the internet is departure time, number of segments, dates until departure, and airline name.
The way im pulling off this information is using SkyScanners API which is avaible on RadarApi. The date of flight is always updated everyday before I run it
because it does 45 days in advance up until one day in advance. The flights are also splitted into 6 different files because the API only accepts 100 requests
per minute which is also why I had a 1 minute hold between each file to avoid receiving a error request. My program.cs file has a lot of room for improvement
if I had more time I would add a try/catch when sending api requests just because sometimes the data doesn't come back correctly and causes the
program to crash
