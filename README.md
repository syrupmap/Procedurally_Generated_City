# (ONGOING PROJECT) Procedurally Generated Cities in Unity 
<img width="521" height="340" alt="Screenshot 2026-07-26 at 11 11 40 PM" src="https://github.com/user-attachments/assets/e80d1d36-403c-48eb-b7b4-174a1549e0ed" />


## About 
This project is a procedural city generator build in Unity using a tile-based constraint system. I plan to use this work for future projects ex: games, videos, and simulations. Each tile defines edge types and the generator fills a grid by selecting tiles that match neighbor edge's. The system uses recursive backtracking to ensure a valid layout.

## Features 
- Procedural city generation
- Configurable generation settings
 
## Built With
- Unity
- C#
- Blender

## Progress (Updating)
### 1) Tile Creation + Development
- Tile-Edge constraints determine how it can connect to neighboring tiles. Tile rotations are automatically supported, increasing layout variety without requiring additional assets. (Current building + road assets Developed/Baked in Blender)
- <img width="650" height="233" alt="Screenshot 2026-07-26 at 11 20 27 PM" src="https://github.com/user-attachments/assets/ce3b84a0-5e2e-4fc0-b983-dcbdfed28103" />
- <img width="400" height="94" alt="Screenshot 2026-07-26 at 11 17 38 PM" src="https://github.com/user-attachments/assets/be268384-a70c-45ba-91c3-d03b4347ce64" />
- <img width="384" height="94" alt="Screenshot 2026-07-26 at 11 18 52 PM" src="https://github.com/user-attachments/assets/91245402-43c7-4f36-84da-1d5df753e2d2" />
- Generator uses recursive backtracking with weighted tile selection to build valid road networks, building blocks, and open spaces while respecting border and connectivity rules.
- <img width="700" height="500" alt="Screenshot 2026-07-19 at 10 00 30 PM" src="https://github.com/user-attachments/assets/d4c0ef29-5542-4d6c-9d4e-b90f26194fff" />

### 2) City Blocking 
- Changed generation system to use a district-based road layout system. Cretes varies urban block structures. First creates network of road bounderies that span between 6-8. The road positions are converted into a grid mask, which determines where roads must exist.
- <img width="521" height="340" alt="Screenshot 2026-07-26 at 11 14 00 PM" src="https://github.com/user-attachments/assets/bcdcece1-78c2-4332-aca0-fa78bee121ab" />
- An issue in the beginning was that the roads would spawn within the blocks and generate these strange shapes, so I created a weighting system to make the probability of roads spawning 5x less likely than buildings.
- <img width="229" height="254" alt="Screenshot 2026-07-26 at 11 15 36 PM" src="https://github.com/user-attachments/assets/09aeb76e-4afa-45f7-b712-9eab60b3082e" /> <img width="229" height="254" alt="Screenshot 2026-07-26 at 11 16 15 PM" src="https://github.com/user-attachments/assets/66215e8f-f716-41ab-b854-7bd7dc7ddd41" />

### 3) Next Steps 
- Optimizing code
- Adding more features like parks + subway sections + third spaces other than buildings and roads


## Resources + Inspiration
- https://www.youtube.com/watch?v=Jsc3BQaJndQ (Inspiration)
- https://www.youtube.com/watch?v=GEi8u6vRENk 

