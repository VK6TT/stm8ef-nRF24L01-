#A barebones nRF24L01+ library for STM8 barebones breakout board

The nRF24L01 registers are 8 bits. Two words nRF@ and nRF! will be used to fetch an 8 bit register and store it back as needed. I simply use pipe 0 ignoring the other pipes. After successfuly swapping data with the default settings I modified them just to see what can be done. 
This code at startup changes the 5 byte address to $E7 D8 C9 B0 A1, sets the power to -18dBm, speed to 250kbps, changes the channel and modifies the number of retries and retry delays. I short, just about everything.

One quirk to be aware of - you can only write command and status registers when in power down or standby modes ie _ce must be low

#Hardware
Connected via SPI the pin _CSN is a chip select not pin ie active low. It must be manually set before SPI tries talking to nRF24 chip. _CE is chip enable pin on nrf24. It is high to enable the RX and toggled low high low to TX a payload.
The _IRQ is an active low interrupt pin. This code at present polls the status register and does not use this interrupt pin.

There are some influence from an amForth library eg _CE where the underscore signifies a pin defined as _CE on the micro. And I used some code from Eelco's io.fs to set up pins as outputs.

Since I don't presently use linux I can't use e4thcom. So I manualy load dependencies or, as I have done here, include them in the file.

#DATA FORMAT
Since we don't always know how many bytes we want to send in a payload ( 1-32) I decided to use a buffer of fixed length = 32 bytes. The whole buffer is sent each time. Thomas has done a great job modifying STM8 eForth so that the buffer can be processed as an input source. Imagine a background task that interprets the buffer and the flexibiity that leads to. No more fixed data formats. You could send two different payoads that are re-ordered but have the same effect for the receiving micro e.g.
Buffer 1 :  30 Temp ! 45 Humidity !
Buffer 2 :  45 30 swap Humidty ! Temp !
A contrived example but one that hopefully alerts you to he potential.

I decided not to store the status byte returned with every instruciton. Instead, whenever I need the status byte I get it. This is a carry over from getting the code to work. 

In the TESTTX example, I used two 10ms delays to give the RX time to process the payload. I'm using 250kbps for my testing and I found a delay was needed when printing out debug information to the terminal. This delay prevents the transmission of a packet before the RX has been turned back on. 

At present I have little error checking. It is a barebones implementation but I hope it serves a usefyul starting point. As I improve the code I will update the library.

Regards
Richard
