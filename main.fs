NVM
#require WIPE
32 CONSTANT P0_WIDTH                 \ bytes in a payload. 1-32 bytes
   VARIABLE MYBUFF P0_WIDTH 1- ALLOT
   VARIABLE RXBUFF P0_WIDTH 1- ALLOT
WIPE

\res MCU: STM8S103
\res export BIT0 BIT1 BIT2 BIT3 BIT4 BIT5 BIT6 BIT7
\res export SPI_CR1 SPI_CR2 SPI_DR SPI_SR
\res export PC_ODR PC_DDR PC_CR1 PC_CR2
\res export PD_ODR PD_DDR PD_CR1 PD_CR2

#require ]B!
#require ]C!

3 CONSTANT _CSN \ pin _CSN on nRF24L01 connected to port D3
2 CONSTANT _CE  \ pin _CE on nRF24L01 connected to port D2

NVM

#require hw/spi.fs

\ timing **************************************************

: *10us  ( n -- )  \  delay n * 10us
   1- FOR [
      $A62B ,    \      LD    A,#42
      $4A  C,    \ 1$:  DEC   A
      $26FD ,    \      JRNE  1$
   ] NEXT
   ;

: ms  ( n -- )  \  delay n ms
   1- FOR 100 *10us NEXT
   ;

: setup_pins  ( -- )
   \ Port C inputs are floating, no interrupts enabled
   [ $0C ( 0b00001100 ) PD_DDR ]C!  \ Port D outputs
   [ $0C                PD_CR1 ]C!  \ set up as push pull outputs
   [ $0C                PD_CR2 ]C!  \ fast mode outputs
   [ $60 ( 0b01100000 ) PC_DDR ]C!  \ port C outputs
   [ $60                PC_CR1 ]C!  \ set as push pull outputs
   [ $60                PC_CR2 ]C!  \ set as fast mode outputs
   ;

: _CE.LOW  ( -- )
   [ 0 PD_ODR _CE ]B!
   ;

: _CE.HIGH  ( -- )
   [ 1 PD_ODR _CE ]B!
   ;

: _CSN.LOW  ( -- )
   [ 0 PD_ODR _CSN ]B!
   ;

: _CSN.HIGH  ( -- )
   [ 1 PD_ODR _CSN ]B!
   ;

WIPE
#include Core

#require PERSIST
PERSIST WIPE
