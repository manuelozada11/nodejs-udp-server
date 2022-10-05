import udp from "dgram";
import express from "express";
import morgan from "morgan";


const HTTP_SERVER_PORT = 5000; // I'm using this port to listen HTTP requests
const UDP_SERVER_PORT = 3000; // locker provider uses this port on board to work (send and receive info)

//  ---- > NOTE: IMPORTANT
//  ** This project doesn't have any architecture
//  ** I recommend follow some architecture for more scalability 

// http server basic setup
const app = express();
app.use(morgan('dev'));
app.use(express.json());
app.post('door/open', (req, res) => {
    const { doorNumber, ipaddr } = req.body;
    console.log(`opening door number ${ Number(doorNumber) } on ip ${ ipaddr }`);

    const door = doorNumber.toString().padStart(2,'0');
    console.log(`trama: 040501${door}${door}`)
    server.send(Buffer.from(`040501${door}${door}`, 'hex').toString('ascii'),'30000',ipaddr,function(error){
        if(error){
            client.close();
        }else{
            console.log('Data sent !!!');
        }
    });
    res.end('opening door')
});
app.listen(HTTP_SERVER_PORT);


// create udp server (he's very similar to websocket or maybe tcp comunications)
const server = udp.createSocket('udp4');

// add event to handle error events
server.on('error', function (err) {
    console.log(`Error: ${err}`);
    server.close();
});


// emits on new datagram msg
server.on('message',function(msg,info){
    console.log('Data received from client: ' + msg.toString());
    console.log('Received %d bytes from %s:%d\n',msg.length, info.address, info.port);
  
    // if I receive some msg then I will send a response msg (like board does it)
    // server.send(Buffer.from('0501010005', 'hex').toString('ascii'),info.port,info.address,function(error){
    //     if(error){
    //         client.close();
    //     }else{
    //         console.log('Data sent !!!');
    //     }
    // });
});


//emits when socket is ready and listening for datagram msgs
server.on('listening',function(){
    // here I made the udp server setup
    const address = server.address();
    const port = address.port;
    const family = address.family;
    const ipaddr = '192.168.13.41';
    console.log('Server is listening at port: ' + port);
    console.log('Server ip: ' + ipaddr);
    console.log('Server is IP4/IP6: ' + family);
});
  
//emits after the socket is closed using socket.close();
server.on('close', function(){
    console.log('Socket is closed !');
});

server.bind(UDP_SERVER_PORT); // here, I'm telling to server which port he will use