NVM
#require WIPE
32 CONSTANT P0_WIDTH                 \ bytes in a payload. 1-32 bytes
   VARIABLE MYBUFF P0_WIDTH 1- ALLOT
   VARIABLE RXBUFF P0_WIDTH 1- ALLOT
WIPE

\res MCU: STM8S103
\res export BIT0 BIT1 BIT2 BIT3 BIT4 BIT5 BIT6 BIT7
\res export SPI_CR1 SPI_CR2 SPI_DR SPI_SR
\res export PB_ODR PB_DDR PB_CR1
\res export PC_ODR PC_DDR PC_CR1 PC_CR2
\res export PD_ODR PD_DDR PD_CR1 PD_CR2
\res export EXTI_CR1 INT_EXTI2

#require ]B!
#require ]C!

5 CONSTANT _LED  \ Led connected to port B5

4 CONSTANT _IRQ  \ pin _IRQ on nRF24L01 connected to port C4

3 CONSTANT _CSN  \ pin _CSN on nRF24L01 connected to port D3
2 CONSTANT _CE   \ pin _CE on nRF24L01 connected to port D2

NVM

#require hw/spi.fs

 \ : SPIon  ( -- )
 \  [ $0C SPI_CR1 ]C!  \ works for me with 4MHz SPI clock
 \  [ $03 SPI_CR2 ]C!  \ no NSS, FD, no CRC, software slave, master
 \  [ $4C SPI_CR1 ]C!  \ enable SPI
 \  ;




\ timing **************************************************

: setup_pins  ( -- )
   [ 1 PB_DDR _LED ]B! \ PB5 debug LED output
   [ 1 PB_CR1 _LED ]B! \ set up as low-side output (PB.5 is "open drain")
   [ $0C ( 0b00001100 ) PD_DDR ]C!  \ Port D setup nRF24L01 outputs
   [ $0C                PD_CR1 ]C!  \ set up as push pull outputs
   [ $0C                PD_CR2 ]C!  \ fast mode outputs
   [ $60 ( 0b01100000 ) PC_DDR ]C!  \ port C: setup SPI outputs
   [ $60                PC_CR1 ]C!  \ set as push pull output
   [ $60                PC_CR2 ]C!  \ set as fast mode outputs
   ;

: IrqInit ( -- )  \  enable nRF24 IRQ interrupt of PC.4
   [ 1 EXTI_CR1 5 ]B!  \  PC falling edge interrupt
   [ 1 PC_CR2 _IRQ ]B! \  PC4 IRQ
   ;

: LED.On  ( -- )  [ 0 PB_ODR _LED ]B! ;
: LED.Off  ( -- ) [ 1 PB_ODR _LED ]B! ;

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

WIPE
#include Core

#require PERSIST
PERSIST WIPE
