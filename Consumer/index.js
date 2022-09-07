const amqp = require("amqplib");
const { Client } = require('whatsapp-web.js');
const qrcode = require('qrcode-terminal');

const consumerName = process.argv[2];

const client = new Client();

client.on('qr', (qr) => {
    // Generate and scan this code with your phone
    qrcode.generate(qr, { small: true });
    console.log('QR RECEIVED', qr);
});

client.on('ready', () => {
    console.log('Client is ready!');
});

client.on('message', msg => {
    if (msg.body == '!ping') {
        msg.reply('pong');
    }
});

client.initialize();



async function connect() {
 try {

   const connection = await amqp.connect("amqp://localhost:5672");
   const channel = await connection.createChannel();
   await channel.assertQueue("MsQueue");
   channel.consume("MsQueue", message => {
    client.sendMessage("50241199517@c.us",`message from consumer ${consumerName}: ${message.content.toString()}`);
    console.log(message.content.toString());
    //  const input = JSON.parse(message.content.toString());
    //  console.log(`Received number: ${input.number}`);
     channel.ack(message);
   });
   console.log(`Waiting for messages...`);
 } catch (ex) {
   console.error(ex);
 }
}


connect();