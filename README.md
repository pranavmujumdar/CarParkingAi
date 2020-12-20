# CarParkingAi
This is a Unity Project I'm working on to create an AI that will park the car by detecting an empty space

Inference of the Training
[Link to the You tube video](https://youtu.be/0G5L-2T0a5s)

## Installation and setup Instructions inside the "ProjectReportAndPresentation" > ParkingAssistAI.pdf

## Specifications
1. Currently the AI is able to find the parking spot and drive to it, the episode resets as soon as the AI collides with the parking spot
2. The empty Spot is spawned randomly on the parking lot
3. The agent car is spawned randomly in the empty central area
4. When the agent collides with any other parked cars it get a reward of (-1.0f)
5. On finding the parking and driving to it, it gets a reward of (5.0f)

## Config, Models and Tensorboard Summaries
Under "AIconfigAndSummaries"

1. trainer_config.yaml for PPO configuration
2. SAC_trainer_config.yaml for SAC configuration
