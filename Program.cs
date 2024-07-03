using System;
using static System.Console;    
enum State{
    Closed, Open
}

/// 
/// My project is to compute the shortest cost to go to the Mala Strana campus from Listopadu domitory. We apply Dijkstra algorithm with binary heap.
/// So, starting vertex is Listopadu, and goal vertex is Mala Strana. 
/// In this code, if we suppose making dictonaries, the graph take constant time, totally it take O((|E| + |V|)log|V|) where |E| is the number of edge, |V| is the number of vertices in the graph.
/// Once we run this code, we will see the shortest path with time and total weight it takes on the terminal. 
/// 
class Schedule
{
    /// 
    /// In this class, we make mainly the dictionaries for the public trasportations namely, Tram 15, 17, 20, 22, 23, Bus 187, 201, Metro red, Green. The reference of this information is https://www.dpp.cz/en/timetables
    /// 
    Dictionary<int, int[]> tram15_schedule = new Dictionary<int, int[]>();
    Dictionary<int, int[]> tram17_schedule = new Dictionary<int, int[]>();
    Dictionary<int, int[]> tram20_schedule = new Dictionary<int, int[]>();
    Dictionary<int, int[]> tram22_schedule = new Dictionary<int, int[]>();
    Dictionary<int, int[]> tram23_schedule = new Dictionary<int, int[]>();
    Dictionary<int, int[]> bus187_schedule = new Dictionary<int, int[]>();
    Dictionary<int, int[]> bus201_schedule = new Dictionary<int, int[]>();
    Dictionary<int, int[]> green_metro_schedule = new Dictionary<int, int[]>();
    Dictionary<int, int[]> tram_m15_schedule = new Dictionary<int, int[]>();
    Dictionary<int, int[]> red_metro_schedule = new Dictionary<int, int[]>();

    void makeSchedule()
    {
        Dictionary<int, int[]>[] list_of_schedule = {tram15_schedule, tram17_schedule, tram20_schedule, tram22_schedule, tram23_schedule, bus187_schedule, 
                                                        bus201_schedule, green_metro_schedule, tram_m15_schedule, red_metro_schedule};
        string[] list_of_txt = {"15.tram.txt", "17.tram.txt", "20.tram.txt", "22.tram.txt", "23.tram.txt", "187.bus.txt", 
                                                        "201.bus.txt", "green.metro.txt", "m15.tram.txt", "red.metro.txt"};
        int count = 0;

        foreach(string text in list_of_txt)
        {
        using StreamReader path = new StreamReader(text);
        while(path.ReadLine() is string prev_split)
        {
            string[] split = prev_split.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            list_of_schedule[count][int.Parse(split[0])] = split.Length != 1 ? split[1 .. ^1].Select(int.Parse).ToArray() : new int[0];
        }
        count++;
        }
    }
    class Graph
    {
        /// <summary>
        /// We suppose the graph is directed and weighted. A node represents where we stop, such as bus stops, statations. The graph is an adjacency matric representation. A edge represents the path between station to station. In this class, we construct the graph from graph_adj.txt, some dictonaries for weight of edges,
        /// the state of nodes, and temp_weight is initial weight. Dictonary h[node] represents the smallest weight from starting vertex to node.
        /// </summary>
        int infinity = 1000000;
        Dictionary<string, string[]> graph = new Dictionary<string, string[]>();
        Dictionary<string, State> graph_state = new Dictionary<string, State>();
        Dictionary<(string, string), int> weight =  new Dictionary<(string, string), int>();
        Dictionary<(string, string), int> temp_weight =  new Dictionary<(string, string), int>();   
        Dictionary<string, int> h =  new Dictionary<string, int>();
        Dictionary<string, string>? prev_node =  new Dictionary<string, string>();

        int num_edge = 0;
        void makeGraph()
        {
            using StreamReader path = new StreamReader("graph_adj.txt");

            while (path.ReadLine() is string str) 
            {
                num_edge++;
                string[] adj = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                graph[adj[0]] = adj[1 .. adj.Length];
                h[adj[0]] = infinity; //plus infinity initially
                prev_node![adj[0]] = null!;
            }
        }
        void makeWeight()
        {
            StreamReader path = new StreamReader("weigth.txt");
            string[] str = path.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            while(str != null)
            {
                weight.Add((str[0], str[1]), int.Parse(str[2]));
                temp_weight.Add((str[0], str[1]), int.Parse(str[2]));
                string temp_str = path.ReadLine()!;

                if(temp_str == null)
                {
                    break;
                }
                else
                {
                    str = temp_str!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                }
            }
        }
        class Time
        {
            /// <summary>
            /// In this class, we set the current time. In process_time fuction, we update the time with edge weight.
            /// </summary>
            int now_hour;
            int now_mins;
            int proceed_mins;
            void setTime(string _now_time, int _hour, int _mins)
            {
                now_hour = _hour;
                now_mins = _mins;
                proceed_mins = _mins;
            }
            (int, int) processTime(int weight)
            { //hour // mins

                if(weight+now_mins >= 60)
                {
                    return (now_hour+1, (weight+proceed_mins)%60);
                }
                else
                {
                    return (now_hour, weight+proceed_mins);
                }
            }

            class Heap
            {
                int left(int i) => 2 * i + 1;
                int right(int i) => 2 * i + 2;
                int parent(int i) => (i - 1) / 2;
                /// <summary>
                /// This is heap sort algorithm. This class has add_value, down_heap, remove smallest, and sorting methods.Sorting takes O(log n). Removing the smallest element takes O(log n)
                /// </summary>
                (string, int)[] ?values; //item1 is the name of vertex, item2 is weigth
                int length;
                Dictionary<string, int>? node_position;
                void minHeap((string, int)[] _values)
                {
                    values = _values;
                    length = 0;
                    node_position = new Dictionary<string, int>();

                }
                void downHeap(int i)
                {

                    if(length > 0)
                    {
                        int left_child = left(i);
                        int right_child = right(i);
                        int smallest = i;

                        (string smallest_node, int smallest_val) = values![smallest];
                        (string node, int node_val) = values![left_child];

                        if(left_child < length && node_val < smallest_val && node != null)
                        {
                            smallest = left_child;
                        }
                        (node, node_val) = values![right_child];
                         (smallest_node, smallest_val) = values![smallest];

                        if(right_child < length && node_val < smallest_val && node != null)
                        {
                            smallest = right_child;
                        }
                        if(smallest != i)
                        {
                            (string, int) temp = values![i];
                            //update dictonary
                            node_position![smallest_node] = i;
                            node_position[temp.Item1] = smallest;
                            
                            values[i] = values[smallest];
                            values[smallest] = temp;

                            downHeap(smallest);
                        }
                    }
                }
                (string, int) removeSmallest()
                {

                    (string, int) x = values![0];
                    values[0] = values[length - 1];
                    length -= 1;
                    //Remove smallest element from dictonary
                    node_position!.Remove(x.Item1);
                    downHeap(0);   
                    values[length] = (null, 0)!;
                    
                    return x;  
                }
               
                void upHeap(int index)
                {
                    while(index > 0 && values![parent(index)].Item2 > values[index].Item2)
                    {
                        
                        (string, int) temp_node = values[index];
                        //update dictonary
                        node_position![values[parent(index)].Item1] = index;
                        node_position[temp_node.Item1] = parent(index);
                        //updade nodes
                        values[index] = values[parent(index)];
                        values[parent(index)] = temp_node;
                        index = parent(index);
                    }
                }
                void decreaseKey((string, int) new_node)
                {
                    (string node_name, int weight) = new_node;

                    if(!node_position!.ContainsKey(node_name))
                    {
                        addValue(new_node);
                        return;
                    }
                    
                    values![node_position[node_name]] = new_node;
                    upHeap(node_position[node_name]);

                }
                void addValue((string, int) new_value)
                {
                    length++;
                    node_position![new_value.Item1] = length -1;
                    values![length -1] = new_value;
                    upHeap(length-1);
                }
                ///
                /// In print_root function, we print out the result of our shortest path from the starting vertex to goal vertex on the terminal. Also, it prints out the total weight time from start to goal.
                ///
                public static void printRoot(List<string> root, Graph graph, Time time_manage)
                {
                    int total_weigth = graph.h["Mala_Strana"] - graph.h["Listopadu"] - (graph.weight[("Listopadu", root[root.Count-1])] - graph.temp_weight[("Listopadu", root[root.Count-1])]);
                    //WriteLine(graph.weight[("Listopadu", root[root.Count-1])] - graph.temp_weight[("Listopadu", root[root.Count-1])]);
                    
                    if(total_weigth >= 10000)
                    {
                        WriteLine("You cannot go to Mala_Strana today.");
                        return;
                    }
                    for(int i = root.Count-1; i >= 0; i--)
                    {
                        if(i == root.Count-1)
                        { 
                            (int hour, int minute) = time_manage.processTime(graph.weight[("Listopadu", root[i])] - graph.temp_weight[("Listopadu", root[i])]);
                            Write($"Listopadu, {hour} : {minute} ---> ");
                            (hour, minute) = time_manage.processTime(graph.h[root[i]]);
                            Write($"{root[i]}, {hour} : {minute} ---> ");
                        }
                        else
                        {
                            (int hour, int minute) = time_manage.processTime(graph.h[root[i]]);
                            if(root[i] == "Mala_Strana")
                            {
                                Write($"{root[i]}. ");
                            }
                            else
                            {
                            Write($"{root[i]},  {hour} : {minute} ---> ");}
                        }
                    }

                    int goal_mins;
                    int goal_hour;

                    if(graph.h["Mala_Strana"]+time_manage.now_mins>=60)
                    {
                        int extrahour = (graph.h["Mala_Strana"]+time_manage.now_mins)/60;
                        goal_mins = (graph.h["Mala_Strana"]+time_manage.now_mins)%60;
                        goal_hour = time_manage.now_hour+extrahour;
                    }
                    else
                    {
                        goal_mins = graph.h["Mala_Strana"]+time_manage.now_mins;
                        goal_hour = time_manage.now_hour;
                    }
                    
                    WriteLine($"Arrive time is {goal_hour} : {goal_mins}");
                    WriteLine($"total weight is {total_weigth}");
                }
                ///
                /// In make_root function, we recurively obtain the nodes included the shortest path from prev_node dictionary. 
                /// Once we reach the undifined node, then go to print_root function.
                ///
                public static void makeRoot(string node, Graph graph, List<string> root, Time time_manage)
                {
                    if(graph.prev_node![node] == null)
                    {
                        printRoot(root, graph, time_manage);
                    }
                    else
                    {
                        root.Add(node);
                        makeRoot(graph.prev_node[node], graph, root, time_manage);
                    }   
                }
                ///
                /// In check_station fucntion, we go to find_nice_shcedule function where the node is received. 
                ///
                
                public static int checkStation(string node, Schedule schedule, Time time_manage, int weight)
                {
                    if(node == "Bus_187") return findNiceSchedule(schedule.bus187_schedule, time_manage, weight, node);
                    else if(node == "Bus_201") return findNiceSchedule(schedule.bus201_schedule, time_manage, weight, node);
                    else if(node == "Tram_17") return findNiceSchedule(schedule.tram17_schedule, time_manage, weight, node);
                    else if(node == "Red_Metro") return findNiceSchedule(schedule.red_metro_schedule, time_manage, weight, node);
                    else if(node == "Green_Metro") return findNiceSchedule(schedule.green_metro_schedule, time_manage, weight, node);
                    else if(node == "mTram_15") return findNiceSchedule(schedule.tram15_schedule, time_manage, weight, node);
                    else if(node == "Tram_20") return findNiceSchedule(schedule.tram20_schedule, time_manage, weight, node);
                    else if(node == "Tram_22") return findNiceSchedule(schedule.tram22_schedule, time_manage, weight, node);
                    else if(node == "Tram_23") return findNiceSchedule(schedule.tram23_schedule, time_manage, weight, node);
                    else if(node == "Tram_15") return findNiceSchedule(schedule.tram15_schedule, time_manage, weight,node);
                    else return 0;
                }
                ///
                /// In find_nice_schedule function, we check the time schedule of tram, metro, or bus. And return the the nicest time plus weight of the edge. 
                /// If we cannnot find it, this total weight becomes inifinty.
                ///
                public static int findNiceSchedule(Dictionary<int, int[]> way, Time time_manage, int weight, string node)
                { //memo: the shcedule is not matched with the result. guess timer is somehow wrong
                    (int hour, int minute) = time_manage.processTime(weight); //hour, mins

                    int[] mins = way[hour];
                    for(int i = 0; i < mins.Length-1; i++)
                    {
                        if(minute == mins[i])
                        {
                            return mins[i] - minute;
                        }
                        if((minute >= mins[i] || minute >= 0)  && minute <= mins[i+1])
                        {
                            return mins[i+1] - minute;
                        }
                    }
                    if(!way.ContainsKey(hour+1) || way[hour+1].Length == 0)
                    {
                        return 100000; //schedule does not exsit, infinity number
                    }
                    return way[hour+1][0] + 60 - minute;
                }
                ///
                /// In dijkstra algorithm, first of all, we set heap and add starting vertex. Then we check every node from the graph and take the smallest weight node from heap.
                /// And at each node, we check every neigbor nodes and do relaxation process. After those processes, we go to make_root function to print out the shortest path.
                /// When we check the nice schedule from the time table of traspotations, 
                /// we update the weight of this node since we need to consider the waiting time, which is that we wait for bus or tram at the node.
                /// 
                
                public static void dijkstra(string start, string goal, Graph graph, Schedule schedule, Time time_manage)
                { //probbably there is error in the metro stuff.
                
                    Heap heap = new Heap();
                    (string, int)[] list_of_nodes = new (string, int)[graph.num_edge];
                    heap.minHeap(list_of_nodes);
                    graph.h[start] = 0;
                    heap.addValue((start, graph.h[start]));

                    foreach(string node in graph.graph.Keys)
                    {
                        graph.graph_state[node] = State.Open;
                        if(heap.length == 0)
                        {
                            List<string> root_a = new List<string>();
                            makeRoot(goal, graph, root_a, time_manage);
                            return;
                        }
                        (string min_next_node, int node_weight) = heap.removeSmallest();
                        
                        foreach(string neighbor in graph.graph[min_next_node])
                        {
                            (string, string) temp_edge = (min_next_node, neighbor);
                            int new_weight = checkStation(neighbor, schedule, time_manage, graph.h[min_next_node] + graph.weight[temp_edge]); // check the closet time
                            graph.weight[temp_edge] += new_weight; //waiting time
                            int temp = graph.h[min_next_node] + graph.weight[temp_edge];
                            if (temp < graph.h[neighbor])
                            {       
                                graph.h[neighbor] = temp;
                                graph.prev_node![neighbor] = min_next_node;
                                heap.decreaseKey((neighbor, temp));
                            }
                        }
                        graph.graph_state[node] = State.Closed;
                    }
                    List<string> root = new List<string>();
                    makeRoot(goal, graph, root, time_manage);
                }
                /// 
                /// In Main function, we obtain current time, and set Time, Graph, Schedule classes, and go to dijstra function with starting and goal vertices. 
                ///    
                public static void Main(string[] args)
                {
                    Time time_manage = new();
                    DateTime dt = DateTime.Now;
                    (int, int)[] test_case = {(9, 0), (10, 0), (11, 0)}; 
                    int now_time_hour = dt.Hour;
                    int now_time_min = dt.Minute;
                    if(args.Length > 0)
                    {
                        switch (args[0])
                        {
                            case "1":
                            now_time_hour = test_case[0].Item1; 
                            now_time_min = test_case[0].Item2; break;
                            case "2":
                            now_time_hour = test_case[1].Item1; 
                            now_time_min = test_case[1].Item2; break;
                            case "3":
                            now_time_hour = test_case[2].Item1; 
                            now_time_min = test_case[2].Item2; break;
                            default:
                            throw new Exception("Please Write correct number.");
                        }
                    }
                    string now = now_time_hour.ToString() + ":" + now_time_min.ToString();
                    time_manage.setTime(now, now_time_hour, now_time_min);

                    Graph graph = new();
                    graph.makeGraph();
                    graph.makeWeight();
                    
                    Schedule schedule = new Schedule();
                    schedule.makeSchedule();
                    dijkstra("Listopadu", "Mala_Strana", graph,schedule,time_manage);
                }
            }
        }
    }
}
