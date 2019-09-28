1. Ajay Bagali, Ritesh Sharma
   Version 2.0 
   December 8th, 2018 

Client README: 
2. Polish/Extra Features: 
	1. We added in a help menu that tells the user the controls and the extra features about the game
	2. On the scoreboard, in the player health bars, there are different colors that show the amount of hit points the player has left.
		-green: 4-5 hit points
		-orange: 2-3 hit points
		-red: 1-2 hit points
	3.The scoreboard also auto updates the player scores and ranks the players based on their scores on a leaderboard type aspect. 
	4. We added in an actual space background to help make look the background of the game a lot better. 

3. When we first read the assignment, it took us a while to understand what was going on because on the length of the assignment. There were a lot of parts that 
   were connected to each other such as network and the game controller. We didn't understand how the form and the drawing panel were connected. Once we rewatched the 
   lectures and understood the lab10 and the fancy chat system lab, we were able to understand how all the networking and the actual game GUI were working. We followed
   the network diagram that was provided on the assignment and were able to get our game working that way. 

4. Design Desicions/Log: 

   11/7/18: 
   We read the assignment specifications once more and we went over the labs to understand how the network controller was working. We sort of drew out how were going
   to connect the the controllers (game/network) with the drawing panel and how that would update the form. Since there were many components, we took it one at a 
   time. We simply set up all the class libraries and set up the network contoller class.
   Time: 1-2 hours 

   11/9/18:
   We started with the SocketState class by making the makesocket and the connecttoserver method by following the lab 10 code. The connectedtocallback was pretty
   tricky because the map wasn't completely showing how to complete the handshake. Once we figured out how the beginReceive and the beginSend, we understood how to
   get the data and how to send the data using the string builder and appending the data. Our sendCallBack basically finishes sending the data. This is useful so we can
   parse data using the JSON stuff and sending controls to the server and auto updating the form. 
   Time:3-4 hours

   11/12/18: 
   Once we understood how the JSON was being represented in our model classes, we implemented the JSON serialization in each of our ship/star/projectile classes
   from the assignment specifications. We made getters/setters for these models. In our world class, we set up 3 dictionaries for each of these game objects. Each
   taking in its game object and the key being its ID being represented by an integer. We also added int size field to keep track of the world size once we parse in that
   second thing from the server. We then started working on the GameController class to figure out how we would represent that data on the form. 
   Time: 3-4 hours

   11/13/18:
   We started working on the GameController class and looked at how the fancy chat system was being implemented. We then looked at how we were reveing data during the
   receive start up and how we were parsing the data and how were removing data from the message buffer. Our receiveWorld method takes in a socket state which passes
   much of the data. We do most of our parsing here and deSerialize the JSON into game objects. After we do this, we then add these objects into our dictionaries in our 
   world. We get the first two bits of data holding the id and the world size in our uniqueIDAndDimensions method.
   Time:2-3 hours

   11/15/18: 
   We implemented the vector2D class. We made a drawing panel class to update the world on the form GUI. Here we copied the object drawer,the drawObjectWithTransform 
   from the lab 10 code to be able use it in our onPaint method. Due to lag, in our game object drawers, we stored most of the images locally in the constructor, so
   we wouldn't have to load the images every time a game object was called. We made gameObject drawers that would draw each of the model game objects with a certain
   offset. We then used these in our onPaint method in a foreach loop which would loop through our world dictionaries that would draw each of the ships/projectiles/and
   star. In our form class, we did the controls and we set up the form aesthetics and adding the drawing panel. We also did the string builder part here, once the user
   inputs controls, we append to the string builder and we send the control data to the server. 
   Time: 4-5 hours

   11/16/18: 
   Once we got the game functionality working, we started to polish and comment the code. We worked on making the menu and the top of the game look a lot better. We added
   in a space background. We then worked on the scoreboard, which took some time because the comparator wasnt working. We were able to draw whitespace on the panel and
   add in a whole bunch of rectangles to help represent the health bar. We used much of the graphics methods to help make it look clean and to add in the text. One feature
   we added in was to update the scoreboard constantly so that the user with the highest score would show up to the top. To do this, we implemented a comparator method in the 
   world class, that would be used to sort the ships by its score in a sorted list dictionary, We then added in a help/ about menu that would  tel  the user what the game was about and 
   how the controls worked. We finished up the read me and commented the code. We then went over everthing and made sure it was all solid. We also added in a aspect where if the user
   would type in an invalid server name, we would pop up a message box saying so and asking the user to reenter a valid server address. 
	
   Time: 5-6 hours
   
Server README:
1.Features/Extra Game Mode: 
	A.GameMode:
		GameMode 0: Normal Game mode
		GameMode 1: Projectiles affected by gravity
					If a ship kills another ship, its health gets fully restored
		In the settings, change the game mode to 0 or 1 depending on the game mode detailed above
	

	B.Features:
		Installer: We made an installer for the client, so that we can send it to other people without access to the visual studio and this project can play spacewars as well
		as long as someone has the client open. 
	 

	
2. Testing:
	Our testing strategy mainly depended on the implementation of our model. All the world objects and the world object
	dictionaries are being stored in the model. Much of the testing simply involved checking to see if our world objects
	were being setup and sent properly to the client. With our projectile class, we tested using by making projectiles and checking
	if its functionality and fields were being set and received properly. With our star class, we made new stars in the world and also checked its
	functionality and fields. Testing the ship class was pretty tricky aside from its functionality and its fields, we included command controls
	in here as well. So we generated new ships and manually set controls for the ships, and tested their outputs and the boolean flags we assigned
	to the ships. 

3. Design Decisions/Log:
	12/3/18: 
	When we first read the assignment all the way through, we were pretty confused becaues of the amount of work needed to be done. We
	looked back at the lecture slides and videos to help guide through on where to start. We then studied the networking digram provided to
	us on the assignment stage and started from there. We started building the code for the networking library. We started working on the
	server awaiting loop with the tcp listener. We made the socketstate callback to pass around data with. We stopped once we finished the 
	HandleNewClient method.

	Time: 3-4 hours

	12/5/18:
	We picked up on the networking code and did the client response stuff such as the receive name method. This method confused us because
	we weren't unsure how to parse the data coming in for each client. We then used the string builder to help send data to a list of clients that we 
	made for each socket state. Once we were able to communicate with the client, we worked on doing the xml reader and settings. We made a settings
	file with all of the game elements used for the world. We set up mulitple readers to read game settings and star settings. With those read in settings
	we set up many fields to easily access the settings.

	Time: 2-3 hours

	12/6/18:
	We started the update method for the game objects. We made a callback method to process data in from the client. This is where we implemented
	the generating of new projectiles. If we read in the fire character, we generate a projectile for that ship along withs its motion and physics.\
	Here in this callback, we also take in client commands processed by our server. These commands are sent into that specific ship set off
	certain boolean flags to help us later with the physics and the motion. We also worked on a id generator for each object to give its own object when
	initialized to that specific id coming in from the socket state.

	Time: 5-6 hours

	12/7/18:
	We continued the update method. We needed to figure out how our ship dictionary would get updated. We added in the physics for the ships,
	so if a boolean flag was ever set off, we would change its direction and location depending on the user input. This would update for every ship. 
	Along with this,we would also take in account for the locations of the ships and make sure it would wrap around if the ship ever went out of bounds. 
	We also worked aorund in our world class on the random ship spawn location and generating the new ship so we could stay consistent with the ship ID's.
	With the projectiles, we made sure to update its location each time and to remove it, if the projectile ever went out of bounds. We then worked 
	on the collisions between the different game objects. This took a while to figure out because of the physics and the motion in calculating the different
	lengths of the objects. We made it into a complex method due to many iterations of all the game objects. Once we got that working, we worked on the projectile timer
	the respawn timer. For the projectiles, we made multiple boolean flags to help us indicate if a ship was firing as well as incrementing its frame to make sure
	it was under the provided frames per shot. We did this similarariliy to the ship respawn counter, we added in a frame counter to whenever a ship dies, it increments
	and it waits a certain time frame provided by the settings to respawn again in a random location. Along with these timers, we also added in the main
	timer for the world's update method. This makes sure all the frames on the world and the game objects are being update in a proper time fashion. We used the
	stopwatch tools provided in Kopta's lecture to help us do this.

	Time: 8-9 hours

	12/8/18:
	Once we got the basic functionality of the game down, we started working on unit testing and the making the other game mode. We worked on handling client disconnection by listening 
	to where the socket would close and setting off a boolean flag for each ship if it would get disconnected or not. We would send in a 0 hp ship to the client to let it know that the
	ship has been disconnected. With our other game mode, we thought of several ways to make it unique. We first thought of working around the projectiles and changing its direction and location
	to be affected by the stars gravity. Our initial thought going into this was to make shooting a bit harder. We also thought of a way to make it longer and more competitive by adding in a 
	healing factor for the ships, so whenever a player kills another ship, it restores that ship back to full hp. We then worked on unit testing. This was pretty difficult because we couldn't really
	test the networking code, so we focused on the world objects in the model. This is all explained in our testing portion on this README. We then finished up by cleaning all the code and adding in
	additional comments to make it more readable. 


	Time: 8-9 hours
	

