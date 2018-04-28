#require WIPE
NVM
32 CONSTANT P0_WIDTH                 \ bytes in a payload. 1-32 bytes
   VARIABLE mybuff P0_WIDTH ALLOT
WIPE

3 CONSTANT _CSN \ pin _CSN on nRF24L01 connected to port D3
2 CONSTANT _CE  \ pin _CE on nRF24L01 connected to port D2

#include Core

#require WIPE
WIPE
