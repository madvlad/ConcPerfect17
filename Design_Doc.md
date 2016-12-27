### Design Document for:

# ConcPerfect &#39;17

The Comprehensive Conc Jumping Game

All work Copyright ©2016 by ConcPerfect Dev Team Studio 96

Written by David Bohan

### Design History

###

 This is a brief explanation of the history of this document, including documented change sets.

 This is a log of changes made to the design document over time. The aim of this section is to provide return readers a quick reference to changed sections from when they last read the document

## Version 1.00

 Version 1.00 is the start of the document. Before that there was only the template.

## Version 1.10

 Version 1.10 – Add details for new focus on gameplay &amp; course development. Major changes include:

- .Preference of random course generation over full course design
- .Switch design focus from overall courses to individual jump components
- .Break gameplay in two: Endless runs and time-based challenges
- .Remove Hearing Impaired accessibility
- .Change the main focus of this game from a story-driven plot to an open-ended personal stat objective
- .Removed Rendering System section
- .Removed &quot;Travel&quot; subsection in the Game World section
- .Remove ability to switch from First Person to Third Person
- .Remove the &quot;World Layout&quot; section

### Game Overview

## Philosophy

### Renew Conc Jumping

 The main focus of this game is to bring the game of conc jumping, as seen in Team Fortress Classic, to a modern platform. This involves recreating the experience as closely as possible but within a new engine (Unity) and with added capabilities.

### Challenging and Competitive

 The game needs to be focused on a competitive format. The courses presented to the player will be difficult enough to provide a fun challenge alone, but the meat of the game will be in various forms of multiplayer competition.

### Clean and Self-aware Design

 The game will be designed in a clean and self-aware manner. The game world and characters within it won&#39;t take themselves too seriously. This is to create a relaxing and not-so-serious atmosphere in the game. The game should go for style and fun, not seriousness and realism.

## Common Questions

### What is the game?

ConcPerfect is a skill based 3D platformer, with some puzzle elements. Players will be faced with a variety of obstacles that they must overcome with the use of a propulsion inducing blast (conc grenade) and careful maneuvering. Some obstacles may be complex and require some basic puzzle solving skills to figure out the correct path to complete it. Timing and precision are two skills this game will test harshly.

### Why create this game?

 We want to create this game because we love conc jumping. We feel that it is a fun and challenging game system that not many people have played because they never owned Team Fortress Classic and joined the few special servers that these games took place on. By creating this game, we hope to give conc jumping a new and larger audience.

### Where does the game take place?

 The game takes place in a number of different courses on the west coast. These range from Montana all the way down to New Mexico. The courses are set in rough and otherwise un-traversable terrain. A course has around nine obstacles each. Each course is manicured and taken care of (think golf courses) and have players with a wide range of skills and ability levels.

### What do I control?

 You will control you, the player. The player is a concer (or: a person who plays the sport of conc jumping) who is trying to &quot;become professional&quot;



### How many characters do I control?

 You will only control one character: you, the player.

### What is the main focus?

 The player is trying to hone their conc jumping skills. Each course they play through will give them practice and experience points that are added to their &quot;stat sheet&quot;.

### What&#39;s different?

There are not too many games out there that are like this exactly. What is different is the usage of a deployable device to enhance the potential movements a player can make. Additionally, the aesthetic the game is taking is a breath of fresh air over what it was back in TFC. Another thing that makes this game different from ones like it (the original conc jumping game mode) is the fact that it will be platform independent, free to play, and offer carefully tailored multiplayer game modes and functionalities.

## Feature Set

##

## General Features

- .45 jump components
- .Randomized courses (Unlimited, 9-jump, or 18-jump)
- .Well defined and smooth conc jumping mechanics
- .3D graphics
- .Customizable character models
- .Multiplayer functionality
- .Ability to customize experience:
  - .FOV customization
  - .Mouse sensitivity customization

## Multiplayer Features

- .Up to 16 players per game
- .Games are found in a server browser
- .Players host their own game servers
- .Chat with voice and text
- .Different multiplayer game modes

## Editor

- .Players will be able to build their own course by selecting and ordering Jump Components manually
- .Given the order of jumps the player should be able to get a seed to share their course with others

## Gameplay

- .Timing functionality to try and beat the courses as fast as possible
- .Challenging courses that are hard enough that merely completing them is fun in itself
- .Online racing functionality to go head to head with your friends
- .Online &quot;skins&quot; game mode that relies more on your skill than your speed



### The Game World

###

## Overview

 The main game world will be divided up into five different courses from different locations in the Western United Stated. Each location has a restricted conc jumping course that the player will be put on. The course will be restricted with physical boundaries like ones seen around the boundaries of a soccer field (advertisements included).



## Difficult and Interesting Terrain Obstacles

 In the world of ConcPerfect there are many difficult obstacles defined by the terrain. The main goal of the entire game is to traverse these obstacles in efficient and fun ways. These obstacles will also be interesting to look at. There will be a certain atmosphere they give off that will make the player want to scale them and get through to the end.

## End-Course Party Rooms or Clubhouses

 It is a custom for each course to have a little room or building after the last obstacle. This room has various cool things in it that players can play around with, and it acts as a reward for players that make it to the end. These are mainly meant for multiplayer games but they work in single player games as well.

## The Physical World

### Overview

 The setting of the physical world takes place on Earth, in the western side of the United States of America. As such a lot of the terrains will be desert and rock based. Much of the physical world that the players will be experiencing will be outside and away from major cities.

### Key Locations

 There are five different jump themes in ConcPerfect &#39;17. They are based on the following locations

- .Taos, New Mexico
- .Pyramid Lake, Nevada
- .Panther Gap, California
- .Carlsbad, New Mexico
- .Monument Valley, Arizona

### Scale

 A course will be made up of 9 or 18 jumps. Each jump can take up anywhere between 200 and 1000 yards. Given these measurements, the scale of each level will be about the size 2 or 3 golf courses.

### Objects

 Players will be able to find packs of concs to replenish their supply back up to the maximum. Players will also see other players and the other players&#39; concs as well. Players may not interact with other players&#39; concs.

### Weather

 Weather will be randomized but will have no effect on gameplay other than atmosphere. At any given time, a course may be loaded with sunny, cloudy, or rainy. Weather will have a chance to change every 5 minutes of actual time.

### Day and Night

 The game will have the ability to change from day to night. Gameplay during day and night are identical, except visibility may be more limited during night (though the courses will be lit up to allow for night time play)

### Time

 Time in the game will be based on real life time. A course should take about an hour to complete given the player is of intermediate or average beginner skill level. After an hour of real life time, day will shift to night. After an additional hour of night time, night will shift back to day.

## Camera

### Overview

The camera will be in the first person perspective.

## Game Engine

### Overview

 The game engine will be based heavily off of GoldSRC in regards to movement logic and air acceleration. A lot of the important features here are inspired by how things worked natively in GoldSRC, which is a game engine developed by Valve initially for Half-Life.

### Bunny Hopping

 The game engine will allow players to bunny hop. Bunny hopping is the ability for a player to continuously jump around non-stop and maintain a high amount of speed.

### Water

 Water will be present in some courses. Players will have the ability to swim up and down and conc under water. Water will slow down any players going through it to a halt.

### Conc Jumping

 The essential game engine feature. There will be logic in the game to handle player propulsion produced by concussive blasts of the conc grenade. Given the distance and angle of the player from the point of origin (the conc grenade), the player will be propelled at a certain velocity and with a certain trajectory. The specifics of this has still yet to be worked out.

### Fluid and Fast Movement

 Most of ConcPerfect&#39;s fun and addictiveness comes from its fast paced game play. In order to achieve an exciting and engaging pacing in the game, the movement of the player needs to be very smooth and fast in an almost arcade-y kind of way. There will be no &quot;heavy feeling&quot; controls.





## Lighting Models

### Overview

 The lighting model used will be Unity&#39;s default lighting model. There will be full use of ambient, diffused, and specular lighting

### Lighting During the Day

 During the day directional lighting will be used. The source of which will be the sun.

### Lighting During the Night

 During the night faint directional lighting will be used, the source of which will be the moon. The more prominent source of lighting at night will be point lighting originating from spot lights around the courses.

### Game Characters

###

## Overview

 ConcPerfect &#39;17 will be limited in the kinds of characters it offers. There will only be the player&#39;s player character and two others for flavor. These two character are the trainer, and Gerald, the announcer / tournament official.

## Creating a Character

 Players will create their player by selecting a base model and supplying a name. They will have the option to customize their model with various items in their inventory, though these changes will only be cosmetic.

### User Interface

###

## Overview

 The UI for ConcPerfect will display all necessary information for gameplay. The UI should be designed to be light and slick, not overloading the player with information but giving them enough information that they don&#39;t have to open up any additional menus to gain any more. Following are individual UI details

## Conc Ammo Count

 There will be a small counter at the bottom right of the screen that will display how many concs there are left in the player&#39;s reserve.

## Conc Primed Notification

 There will be a small icon that appears just above the ammo count that will show the player that they have a conc primed and in hand. A primed conc is one set to go off in the next four seconds.

## Time Left

 There will be a small bar at the top center of the screen displaying how much time there is left to complete the course. If a time limit is not set in the game properties, this will display 0:00.

## Timer

 In timed game play, a small timer will appear below the Time Left notification, showing the player how much time he has spent playing the course since he started his timer.

## Chat Box

 In multiplayer games, chat messages will appear in the bottom left portion of the screen. The player can press enter to begin typing a message to other players in the server.

## Player Level Progression

 In the bottom left of the screen, a number will be displayed telling the player what obstacle they are on and how many obstacles the course has in total.

## Other Player Level Progression

 In the bottom center of the screen will be a row of boxes, each box representing a jump on the course. Inside each box will be any players on that specific jump at that time. Using this, players can track other players&#39; progression through the course.

## Player List

 Pressing tab will bring up a list of all players in the server.



### Musical Scores and Sound Effects

## Overview

 Custom music will be created by Jacob Alfaro. There will be a few tunes for the game, namely the title screen music and the end-course music. Additionally, there will be a large emphasis on nature ambience in the courses themselves. No music will be played while in the course.

## Title Screen Score

 The title screen sequence will be a downbeat and modern jazzy tune that brings to mind &quot;early 2000s professionalism and progress&quot;. See the Windows XP installation music for what I mean with that. A song that will be used as a creative starting point is the song &quot;Dimensional&quot; by Robb Warren. Listen to that here: [https://soundcloud.com/lemoncellomusic/dimensional](https://soundcloud.com/lemoncellomusic/dimensional)

## End Course Score

 The end course sequence will be an upbeat &quot;feel good&quot; song, like the high score from the Commodore 64 game Commando composed by Rob Hubbard. For an acoustic rendition of that song see the beginning of this song: [https://www.youtube.com/watch?v=IdtAUk\_-dv0](https://www.youtube.com/watch?v=IdtAUk_-dv0)  And you can hear the original version of that song here: [https://www.youtube.com/watch?v=hLp8ErRj8s0](https://www.youtube.com/watch?v=hLp8ErRj8s0)

## Ambient Nature Sound Design

 For the ambient nature sounds (being recordings of crickets, birds, water, crowd reactions, etc.) I will be creating my own original recordings for these using a high quality microphone and Audacity for the remastering software. Additionally, the game will make use of royalty free recordings provided by [http://freesound.org](http://freesound.org/)

## Game Mechanic Sounds

 There are a few abstract mechanics that require sound design as well. The concs themselves will have a concussive blast, and a timer beep. Players will produce sound when jumping and walking around.

### Single-Player Game

###

## Overview

 The single-player game is presented in a simple level progression format. The player is initially given one open course to complete, and upon completing that course opens up a new course and so on until they reach the last course.

## Story

 ConcPerfect &#39;17&#39;s story is about an amateur concer who wants to go professional. The only way for him to do that is to ascend the ranks and win tournaments at each of the courses available to him. At the last course he is presented with the Professional Qualifiers tournament: The Jerome Cup. Completing the Jerome Cup will earn him status as a professional.

## Hours of Gameplay

 Given the mean time of completing a course for the first time is around four hours, the expected amount of gameplay one will receive from their play through of the single-player campaign alone is 20 hours.

## Victory Conditions

 The player will win singe-player campaign when they progress to the last stage and win the Jerome Cup.

### Multiplayer Game

###

## Overview

 Multiplayer games will be independently hosted and will have a variety of game modes to choose from. These game modes include: practice, combined racing, separated racing, skins, and relay.

## Max Players

 A server can hold at max sixteen players at a time.

## Servers

 Servers will be hosted independently by anyone who wishes to host their own.

## Internet

 A master list server will be set up by the ConcPerfect &#39;17 team. Any servers hosted will ping that server with their information for it to be displayed on the server list in the game. Clients request a list from the list server and are able to select which ones they wish to join from there.

## Persistence

 The game world will be persistent independent on individual player sessions.

### Custom Course Creation

###

## Overview

 In the future, a course editor will be released for ConcPerfect &#39;17. Course editing and creation is crucial to the life span of ConcPerfect &#39;17 as it relies heavily on new courses and challenges being available to the player.

## Creating from Scratch

 Taking some tips from Valve&#39;s hammer editor and also Portal 2&#39;s snap maps, there will be a simple and easy to use editor for creating brand new courses from scratch. The tool will be easy enough to use so that getting into it doesn&#39;t require a steep learning curve, but it will be feature rich enough to provide creators with tools to create unique and interesting courses.

## Importing Existing bsp Files

 There exists a large repository of conc maps map for TFC that are compiled under the bsp format. Writing an importer to bring those maps into this game and have them playable will be a great feature and will be a huge plus for advancing the life span of ConcPerfect &#39;17.



### The Mechanics of Concing

###

## Overview

This section will describe, in an abstract manner, how concing should work in an ideal system.

## Physical properties of a Conc

The conc is a small rectangular prism 6&quot;x4&quot;x4&quot;. They are small enough to be held in the player&#39;s hand.

 ![](data:image/*;base64,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)

## Basic Conc Mechanics

A conc has a four second fuse. When armed the conc becomes primed and remains in the player&#39;s hand as long as the player is still holding down the prime key. When the player lets go of the prime key, the conc is released from the player becomes an object in the world. Every second after becoming armed the conc plays a beep. The fourth beep is three fast consecutive beeps followed immediately by the explosion of the conc. There is a required 0.5 second delay between the deploy of one conc and the priming of another.

Once exploded, a conc generates a sphere of influence around itself. The conc acts as the point of origin.

 ![](data:image/*;base64,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)

In that illustration, the dot is the conc. Any player outside of the sphere&#39;s boundaries is not effected by the blast at all. Any player inside the sphere _is_ affected by the blast. Direction and velocity can be thought of as vectors and scalars. Upon detonation the conc emits force vectors in all directions. Each vector that hits the player effects the player&#39;s momentum. Thus, the closer the player is to the center of the sphere, the higher the amount of vectors apply to him. Vectors can cancel each other out, so a player close to the center may receive momentum in both the positive and negative direction creating a net momentum gain close to zero or mostly upwards.

A player will want to ideally place themselves as close to the perimeter of the sphere as possible, and position their selves in the angle that they desire to travel to get the most amount of force in the correct direction.



## Behavior of a Conc When Hand Held

A conc may explode while still being in the hand of a player. If this is the case, the explosion logic is dependent on the current velocity of the player. The conc will act as a multiplier of sorts in these situations. If the player is at the peak of their jump and has started accelerating downward, then the conc will blast them downward. Conversely, if the player still has upwards momentum the conc will push the player upwards, providing additional acceleration.
