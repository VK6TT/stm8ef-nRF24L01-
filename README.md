# stm8ef-nRF24L01-
To allow the stm8ef barebones board to interface with a nRF24L01

This is a minimalist implementation for a STM8 on a barebones breakout board

In general the nRF24L01 registers are 8 bits. 
So two words R@ and R! will be used to fetch an 8 bit register and store it back as needed
Simply using pipe 0 ignoring the other pipes
I used the 5 byte address $E7 D8 C9 B0 A1
left everything at default where possible

# hardware
connected via SPI
the pin _CSN is a chip select not pin. ie active low Must be manually set before SPI tries talking ot nRF24 chip
_CE is chip enable pin on nrf24. active low
_IRQ active low interrupt pin
this code at present polls the interrupts

Influenced by an amForth library I looked at but I chose to develop my own implementation. Everyone has there own style!
