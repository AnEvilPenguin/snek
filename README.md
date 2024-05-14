# Snek

This is my take on the classic mobile game Snake.  
The player will try to guide the snake to eat as many mice as possible. 
The snake will move a singe cell in a selected direction periodically. 
Every 5 mice the period in which the snake moves will reduce. 
If no direction is selected (or the exact opposite direction to that last moved is selected) the snake will continue in a straight line. 
The game is over if the snake eats its own tail, or leaves the map.

## Project Synopsis

This is my first full game made from scratch without following tutorials.  
The objective was mostly to have a structured opportunity to be creative. The rules of the game are the same as the original version of snake.

## Tools

- Godot 4.2
- Aseprite 1.3.6
- Visual Studio Community 2022
- Audacity 3.5.1

## Controls

Enter to start game
Arrow keys to control direction.

Controller support should also work, however this has only been tested against a knock off Xbox contoller.

# Closing thoughts

The project generally went well. I'm generally pleased with how things turned out.
It took me a while to work out that child nodes were always positioned relative to their parent. I'd been working on the assumption that they were relative to the main scene, almost like a canvas.
Largely things worked as I expected they would.
If I were to re-do things I think I would define an explicit grid (say an array of arrays) to keep track of where spaces were free or not. Checking for overlapping areas worked well for detection of eating or game over events. However it did make placing a new mouse significantly harder than I expected.
I enjoyed working in Aseprite far more than I expected to. I was originally planning on a very simplistic set of sprites, but I wound up with a much more complex set (that I am far happier with). There's still room for improvement here (I'm not sure I have the shading quite down yet), however I did learn a lot.

I half wanted to have the snake hatch out of an egg before moving off, however this would significantly delay the completion of the game.
I may look into this in future.
