![ConcPerfect Dev Team Logo](http://i.imgur.com/UU4mK3Q.jpg)

### Design Document for:

# ConcPerfect &#39;17

The Comprehensive Conc Jumping Game

All work Copyright ©2016 by ConcPerfect Dev Team Studio 96

## Design History

 This is a brief explanation of the history of this document, including documented change sets.

 This is a log of changes made to the design document over time. The aim of this section is to provide return readers a quick reference to changed sections from when they last read the document

## Version History

### Version 1.00

 Version 1.00 is the start of the document. Before that there was only the template.

### Version 1.10

 Version 1.10 – Add details for new focus on gameplay &amp; course development. Major changes include:

- Preference of random course generation over full course design
- Switch design focus from overall courses to individual jump components
- Break gameplay in two: Endless runs and time-based challenges
- Remove Hearing Impaired accessibility
- Change the main focus of this game from a story-driven plot to an open-ended personal stat objective
- Removed Rendering System section
- Removed &quot;Travel&quot; subsection in the Game World section
- Remove ability to switch from First Person to Third Person
- Remove the &quot;World Layout&quot; section

# Game Overview

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

# Feature Set

## General Features

- 45 jump components
- Randomized courses (Unlimited, 9-jump, or 18-jump)
- Well defined and smooth conc jumping mechanics
- 3D graphics
- Customizable character models
- Multiplayer functionality
- Ability to customize experience:
  - FOV customization
  - Mouse sensitivity customization

## Multiplayer Features

- Up to 16 players per game
- Games are found in a server browser
- Players host their own game servers
- Chat with voice and text
- Different multiplayer game modes

## Editor

- Players will be able to build their own course by selecting and ordering Jump Components manually
- Given the order of jumps the player should be able to get a seed to share their course with others

## Gameplay

- Timing functionality to try and beat the courses as fast as possible
- Challenging courses that are hard enough that merely completing them is fun in itself
- Online racing functionality to go head to head with your friends
- Online &quot;skins&quot; game mode that relies more on your skill than your speed

# The Game World

## Overview

 The main game world will be divided up into several different courses themed from different locations in the Western United Stated. Each location has a restricted conc jumping course that the player will be put on. The course will be restricted with physical boundaries like ones seen around the boundaries of a soccer field (advertisements included).

## Difficult and Interesting Terrain Obstacles

 In the world of ConcPerfect there are many difficult obstacles defined by the terrain. The main goal of the entire game is to traverse these obstacles in efficient and fun ways. These obstacles will also be interesting to look at. There will be a certain atmosphere they give off that will make the player want to scale them and get through to the end.

## End-Course Party Rooms or Clubhouses

 It is a custom for each course to have a little room or building after the last obstacle. This room has various cool things in it that players can play around with, and it acts as a reward for players that make it to the end. These are mainly meant for multiplayer games but they work in single player games as well.

## The Physical World

### Overview

 The setting of the physical world takes place on Earth, in the western side of the United States of America. As such a lot of the terrains will be desert and rock based. Much of the physical world that the players will be experiencing will be outside and away from major cities.

### Key Locations

 There are five different jump themes in ConcPerfect &#39;17. They are based on the following locations

- Taos, New Mexico
- Pyramid Lake, Nevada
- Panther Gap, California
- Carlsbad, New Mexico
- Monument Valley, Arizona

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

 Water will be present in some courses. Players will have the ability to swim up and down and conc under water. Water will slow down any players going through it to a halt. Water will be made from scratch following the guide on Unity's website.

### Conc Jumping

 The essential game engine feature. There will be logic in the game to handle player propulsion produced by concussive blasts of the conc grenade. Given the distance and angle of the player from the point of origin (the conc grenade), the player will be propelled at a certain velocity and with a certain trajectory.

### Fluid and Fast Movement

 Most of ConcPerfect&#39;s fun and addictiveness comes from its fast paced game play. In order to achieve an exciting and engaging pacing in the game, the movement of the player needs to be very smooth and fast in an almost arcade-y kind of way. There will be no &quot;heavy feeling&quot; controls.

## Lighting Models

### Overview

 The lighting model used will be Unity&#39;s default lighting model. There will be full use of ambient, diffused, and specular lighting

### Lighting During the Day

 During the day directional lighting will be used. The source of which will be the sun.

### Lighting During the Night

 During the night faint directional lighting will be used, the source of which will be the moon. The more prominent source of lighting at night will be point lighting originating from spot lights around the courses.

# Game Characters

## Overview

 ConcPerfect &#39;17 will be limited in the kinds of characters it offers. There will only be the player&#39;s player character and two others for flavor. These two character are the trainer, and Gerald, the announcer / tournament official.

## Creating a Character

 Players will create their player by selecting a base model and supplying a name. They will have the option to customize their model with various items in their inventory, though these changes will only be cosmetic.

### User Interface

## Overview

 The UI for ConcPerfect will display all necessary information for gameplay. The UI should be designed to be light and slick, not overloading the player with information but giving them enough information that they don&#39;t have to open up any additional menus to gain any more. Following are individual UI details

## Conc Ammo Count

 There will be a small counter at the bottom right of the screen that will display how many concs there are left in the player&#39;s reserve.

## Conc Primed Notification

 There will be a small icon that appears just above the ammo count that will show the player that they have a conc primed and in hand. A primed conc is one set to go off in the next four seconds.

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

# Musical Scores and Sound Effects

## Overview

 Custom music will be created by Jacob Alfaro. Music will be available to the player on demand in a small jukebox like feature. Additionally, there will be a large emphasis on nature ambience in the courses themselves.
 
## Title Screen Score

 The title screen sequence will be a downbeat and modern jazzy tune that brings to mind &quot;early 2000s professionalism and progress&quot;. See the Windows XP installation music for what I mean with that. A song that will be used as a creative starting point is the song &quot;Dimensional&quot; by Robb Warren. Listen to that here: [https://soundcloud.com/lemoncellomusic/dimensional](https://soundcloud.com/lemoncellomusic/dimensional)

## End Course Score

 The end course sequence will be an upbeat &quot;feel good&quot; song, like the high score from the Commodore 64 game Commando composed by Rob Hubbard. For an acoustic rendition of that song see the beginning of this song: [https://www.youtube.com/watch?v=IdtAUk\_-dv0](https://www.youtube.com/watch?v=IdtAUk_-dv0)  And you can hear the original version of that song here: [https://www.youtube.com/watch?v=hLp8ErRj8s0](https://www.youtube.com/watch?v=hLp8ErRj8s0)

## Ambient Nature Sound Design

 For the ambient nature sounds (being recordings of crickets, birds, water, crowd reactions, etc.) I will be creating my own original recordings for these using a high quality microphone and Audacity for the remastering software. Additionally, the game will make use of royalty free recordings provided by [http://freesound.org](http://freesound.org/)

## Game Mechanic Sounds

 There are a few abstract mechanics that require sound design as well. The concs themselves will have a concussive blast, and a timer beep. Players will produce sound when jumping and walking around.

# Single-Player Game

## Overview

 The single-player portion of the game is strictly a stats building game. Over time you get better and better scores based on how fast you complete a course, how many courses you have completed, and how many jumps you complete each time you do an event. There is no classic story line.
 
 Gameplay is randomized so for single player the game lasts more as a hobby and less as a game to go through from start to end.

# Multiplayer Game

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

# The Mechanics of Concing

## Overview

This section will describe, in an abstract manner, how concing should work in an ideal system.

## Physical properties of a Conc

The conc is a small rectangular prism 6&quot;x4&quot;x4&quot;. They are small enough to be held in the player&#39;s hand.

![Image of a Conc](http://i.imgur.com/MB6iSkl.png)

## Basic Conc Mechanics

A conc has a four second fuse. When armed the conc becomes primed and remains in the player&#39;s hand as long as the player is still holding down the prime key. When the player lets go of the prime key, the conc is released from the player becomes an object in the world. Every second after becoming armed the conc plays a beep. The fourth beep is three fast consecutive beeps followed immediately by the explosion of the conc. There is a required 0.5 second delay between the deploy of one conc and the priming of another.

Once exploded, a conc generates a sphere of influence around itself. The conc acts as the point of origin.

![Sphere Radius](http://i.imgur.com/fQC9s44.png)

In that illustration, the dot is the conc. Any player outside of the sphere&#39;s boundaries is not effected by the blast at all. Any player inside the sphere _is_ affected by the blast. Direction and velocity can be thought of as vectors and scalars. Upon detonation the conc emits force vectors in all directions. Each vector that hits the player effects the player&#39;s momentum. Thus, the closer the player is to the center of the sphere, the higher the amount of vectors apply to him. Vectors can cancel each other out, so a player close to the center may receive momentum in both the positive and negative direction creating a net momentum gain close to zero or mostly upwards.

A player will want to ideally place themselves as close to the perimeter of the sphere as possible, and position their selves in the angle that they desire to travel to get the most amount of force in the correct direction.

## Behavior of a Conc When Hand Held

A conc may explode while still being in the hand of a player. If this is the case, the explosion logic is dependent on the current velocity of the player. The conc will act as a multiplier of sorts in these situations. If the player is at the peak of their jump and has started accelerating downward, then the conc will blast them downward. Conversely, if the player still has upwards momentum the conc will push the player upwards, providing additional acceleration.
