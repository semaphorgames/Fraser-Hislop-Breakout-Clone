# Fraser-Hislop-Breakout-Clone
A clone of Atari's Breakout

Project Trello: https://trello.com/b/zOcmvmCT/breakout-clone

# Feature Details

## Dynamic Brick Size, Rows and Columns

BrickController in the Scene View has attributes to modify the number of rows and columns of bricks, as well as the gap size between bricks

SpawnZoneTL (top-left) and SpawnZoneBR (bottom-right) determine the bounds in which bricks are spawned, bricks are automatically sized accordingly

## Multiple Rounds

Clearing all bricks increases the speed of the ball, and replaces the bricks

This can be easily tested by setting the number of rows and columns to a low number
