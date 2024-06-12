# The Shortest Path for My Life  
### This project is mainly to compute the shortest path (time) to go to the my campus by public transpotations, called Mala Strana, to my dorminory, called Listopadu, in Prague, Czecha.  
## Back Ground. 
### When I go to my campus, I usually search the path by Google Map. However, this does not make the actuall shortest path. Thus, I decided to do this project in order to make my life easier.  
## Description  
### I apply Dijkstra algorithm with binary heap to obtain the result. Let us consider that G is the directed weighted graph and ```G(E,V)```, for all edges in E and virtices in V. In binary heap, Insert/decreaseKey operation takes ```O(log |V|)``` and delete the smallest number from this heap takes ```O(|V|)```. We assume that Vertices are Listopadu (starting vertex), Mara Strana (goal vertex), bus stops, stations, ,and metros. Also, weight is the costs which are by walks, buses, metro, trams, and waiting time. We can see visually the whole graph below as a picture.  
## How to Run this code  
### We simply do ```dotnet run``` on the terminal in the currect directory.
```
Listopadu, 14 : 56 ---> Tram_17, 15 : 10 ---> Tram_15,  15 : 24 ---> Mala_Strana. Arrive time is 15 : 28
total weight is 39
```
#### If you would like to see the detail of the code, I have written them into my code.  
#### Thank you for reading it.  
##### Yoshiki Sakurai
