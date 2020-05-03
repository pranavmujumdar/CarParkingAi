# CarParkingAi
This is a Unity Project I'm working on to create an AI that will park the car by detecting an empty space

Inference of the Training
![Gif](https://media.giphy.com/media/ThuZCwh7Va9rBUW82O/giphy.gif)

## Specifications
1. Currently the AI is able to find the parking spot and drive to it, the episode resets as soon as the AI collides with the parking spot
2. The empty Spot is spawned randomly on the parking lot
3. The agent car is spawned randomly in the empty central area
4. When the agent collides with any other parked cars it get a reward of (-1.0f)
5. On finding the parking and driving to it, it gets a reward of (5.0f)
