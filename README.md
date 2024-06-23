# CaptureTheFlag

![Alt text](CTF2.png?raw=true "Title")

AI Capture the Flag is a multi-objective reinforcement learning experiment understanding the interactions between multiple agents in the same environment, involving teamwork and cooperation.

Each team consists of three agents that will work together to capture the opposing team's flag located on the other side of the arena. Agents must grapple between two objectives, capturing the flag and not letting the opponents capture theirs.

![Alt text](CTF_1.gif?raw=true "Title")
We can see here that in one epoch, the red team chooses to go for the flag, while in the other epoch they choose to defend against the blue agents.

The agents are trained using Proximal Policy Optimization (PPO), a popular reinforcement learning algorithm introduced by OpenAI. PPO uses multiple epochs of stochastic gradient ascent to perform policy updates, allowing for faster and more effective training.

Rewards and punishments were given based on soft-objectives that were tuned to create favorable behaviors, such as capturing the flag and tagging opponents. 

![Alt text](CTF_3.gif?raw=true "Title")
Early iteration of training when agents start to find rewards in opposing territory.

Some other interesting behaviors were seen during training. In one round of testing, there was an instance of agents using the obstacles as shields to hide behind in order to not get tagged.

![Alt text](CTF_2.gif?raw=true "Title")
Red agents using the obstacles to hide from the blue agents"

Another behavior seen that impacted the training was that agents preferred the edges of the arena even when the middle was free due to a cluster of opposing agents on the other side.

![Alt text](CTF_Mov.gif?raw=true "Title")

I realized that the agents in this iteration were not learning the game, but the positions of where the opponents would be. To solve this I implemented a type of data augmentation by rotation the arena environment.

![Alt text](CTF_A2.gif?raw=true "Title")

This prevented the agents from abusing their positions in the environment locally, instead now having to learn based on where the opponents were.

One note is that these agents were not trained for long compared to the amount of iterations an OpenAI paper based on multi-agent game play in a hide and seek environment had. (https://openai.com/research/emergent-tool-use) OpenAI used about ~500 million episodes as compared to ~80 million episodes in which these agents were trained. Partially this is due to the massive amounts of resources and time needed to properly train reinforcement learning agents. Especially with multiple agents in a multi-objective game environment, proper training time is important to gain better results.
