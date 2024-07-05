# The Shortest Path for My Life  
### This project is mainly to compute the shortest path (time) to go to the my campus by public transpotations, called Mala Strana, to my dorminory, called Listopadu, in Prague, Czecha.  
## Back Ground. 
#### When I go to my campus, I usually research the path by Google Map. However, this does not make the actuall shortest path. Thus, I decided to do this project in order to make my life easier.  
## Description  
#### I apply Dijkstra algorithm with binary heap to obtain the result. Let us consider that G is the directed weighted graph and ```G(E,V)```, for all edges in E and virtices in V. In binary heap, Insert/decreaseKey operation takes ```O(log |V|)``` and delete the smallest number from this heap takes ```O(|V|)```. Thus, this algorithm totally takes ```O((|E| + |V|)log|V|)```. We assume that Vertices are Listopadu (starting vertex), Mara Strana (goal vertex), bus stops, stations, ,and metros. Also, weight is the costs which are by walks, buses, metro, trams, and waiting time. We can see visually the whole graph below as a picture.  
## How to work this project
### There are two cases that we can run this project
### First, we can simply run the command line
```
dotnet run
```
### Then this code obtains current time and produces the standard output. There is the example below when we run this code at 8 : 30.
```
Listopadu, 8 : 32 ---> Tram_17, 8 : 46 ---> Tram_15,  9 : 0 ---> Mala_Strana. Arrive time is 9 : 4
total weight is 32
```
### Second, we pass three tastcases as the first command line argument:```1```, ```2``` or, ```3```.
### ```dotnet run 1 ``` We check the test case that is 9:00. The result is below.
```
Listopadu, 9 : 5 ---> Bus_187, 9 : 10 ---> Nádraží_Holešovice,  9 : 15 ---> Red_Metro,  9 : 19 ---> Museum,  9 : 25 ---> Green_Metro,  9 : 28 ---> Malostranska,  9 : 32 ---> Tram_22,  9 : 33 ---> Mala_Strana. Arrive time is 9 : 36
total weight is 31
```
### For the testcase ```2```:  this is 10:00, We will obtain the result below.
```
Listopadu, 10 : 5 ---> Bus_187, 10 : 10 ---> Nádraží_Holešovice,  10 : 15 ---> Red_Metro,  10 : 20 ---> Museum,  10 : 26 ---> Green_Metro,  10 : 29 ---> Malostranska,  10 : 33 ---> Tram_20,  10 : 35 ---> Mala_Strana. Arrive time is 10 : 38
total weight is 33
```

### For the testcase ```3``` : this is 11:00. We will obtain the result below.
```
Listopadu, 11 : 5 ---> Bus_187, 11 : 10 ---> Nádraží_Holešovice,  11 : 15 ---> Red_Metro,  11 : 19 ---> Museum,  11 : 25 ---> Green_Metro,  11 : 29 ---> Malostranska,  11 : 33 ---> Tram_20,  11 : 35 ---> Mala_Strana. Arrive time is 11 : 38
total weight is 33
```

#### If you would like to see the detail of the code, I have written them into my code.  
#### Thank you for reading it. 

##### Yoshiki Sakurai
![graph](https://github.com/yoshiyan1001/The-Shortest-Path-for-My-Life/assets/84613132/e8134fdf-ea76-4ef6-8c96-fb69b6fd473f)
