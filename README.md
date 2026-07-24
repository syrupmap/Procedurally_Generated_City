# (ONGOING PROJECT) Procedurally Generated Cities in Unity 
<img width="1164" height="806" alt="Screenshot 2026-07-24 at 12 44 56 AM" src="https://github.com/user-attachments/assets/48757941-db29-491a-b878-5df3a2bee148" />
<img width="609" height="406" alt="Screenshot 2026-07-24 at 12 41 25 AM" src="https://github.com/user-attachments/assets/e3e768dc-b589-42f5-9264-c8d839a578c3" />

## About 
This project is a procedural city generator build in Unity using a tile-based constraint system. I plan to use this work for future projects ex: games, videos, and simulations. Each tile defines edge types and the generator fills a grid by selecting tiles that match neighbor edge's. The system uses recursive backtracking to ensure a valid layout.

## Features 
- Procedural city generation
- Configurable generation settings
 
## Built With
- Unity
- C#

## Progress (Updating)
### Tile Creation + Development
- Tile-Edge constraints determine how it can connect to neighboring tiles. Tile rotations are automatically supported, increasing layout variety without requiring additional assets.
<img width="500" height="250" alt="IMG_CE217C337B09-1" src="https://github.com/user-attachments/assets/dc4db10e-546b-4d05-ba26-c400aabdb748" />
<img width="500" height="350" alt="IMG_0A79C80EBA14-1" src="https://github.com/user-attachments/assets/8ed92f9d-6161-4b0e-976b-3305e5ec976b" />

- Generator uses recursive backtracking with weighted tile selection to build valid road networks, building blocks, and open spaces while respecting border and connectivity rules.
    - Colored <img width="700" height="575" alt="Screenshot 2026-07-19 at 10 00 30 PM" src="https://github.com/user-attachments/assets/d4c0ef29-5542-4d6c-9d4e-b90f26194fff" />
    - Currently using simple blender assets <img width="793" height="441" alt="Screenshot 2026-07-19 at 10 33 40 PM" src="https://github.com/user-attachments/assets/6f25d636-9cbf-4874-8666-ae74a575245f" />

## City Blocking 
- 

## Resources + Inspiration
- https://www.youtube.com/watch?v=Jsc3BQaJndQ (Inspiration)
- https://www.youtube.com/watch?v=GEi8u6vRENk 

