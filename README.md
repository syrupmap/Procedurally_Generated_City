# (ONGOING PROJECT) Procedurally Generated Cities in Unity 
<img width="1507" height="875" alt="Screenshot 2026-07-19 at 10 00 30 PM" src="https://github.com/user-attachments/assets/97c381eb-402d-4f14-82f7-1e18f5d71efb" />

## About 
This project is a procedural city generator build in Unity using a tile-based constraint system. I plan to use this work for future projects ex: games, videos, and simulations. Each tile defines edge types and the generator fills a grid by selecting tiles that match neighbor edge's. The system uses recursive backtracking to ensure a valid layout.

## Features 
- Procedural city generation
- Configurable generation settings
 
## Built With
- Unity
- C#

## Current Progress
- Tile-Edge constraints determine how it can connect to neighboring tiles. Tile rotations are automatically supported, increasing layout variety without requiring additional assets.
<img width="2176" height="1245" alt="IMG_CE217C337B09-1" src="https://github.com/user-attachments/assets/dc4db10e-546b-4d05-ba26-c400aabdb748" />
- Generator uses recursive backtracking with weighted tile selection to build valid road networks, building blocks, and open spaces while respecting border and connectivity rules.
    - Non-Colored <img width="1507" height="875" alt="Screenshot 2026-07-19 at 9 24 55 PM" src="https://github.com/user-attachments/assets/8ee1c070-eed2-4022-b979-98b918752d34" />
    - Colored <img width="1507" height="875" alt="Screenshot 2026-07-19 at 10 00 30 PM" src="https://github.com/user-attachments/assets/d4c0ef29-5542-4d6c-9d4e-b90f26194fff" />
- Currently using simple blender assets <img width="793" height="441" alt="Screenshot 2026-07-19 at 10 33 40 PM" src="https://github.com/user-attachments/assets/6f25d636-9cbf-4874-8666-ae74a575245f" />

- Next Steps... Working on improving the tile spawning

## Resources + Inspiration
- https://www.youtube.com/watch?v=Jsc3BQaJndQ (Inspiration)
- https://www.youtube.com/watch?v=GEi8u6vRENk 

